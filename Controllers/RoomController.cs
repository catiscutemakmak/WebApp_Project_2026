using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;
using hateekub.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using hateekub.Hubs;
using Microsoft.EntityFrameworkCore;

[Route("game/{gameName}/room")]
public class RoomController : Controller
{   
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    private readonly IHubContext<RoomHub> _hub;

    public RoomController(AppDbContext context, UserManager<IdentityUser> userManager,IHubContext<RoomHub> hub)
    {
        _context = context;
        _userManager = userManager;
        _hub = hub;
    }

    // หน้า View ของ Room
[HttpGet("{roomId}")]
public async Task<IActionResult> Room(string gameName, int roomId)
{
    var currentUser = await _userManager.GetUserAsync(User);

    if (currentUser == null)
        return RedirectToAction("Login", "Account");

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

    if (userProfile == null)
        return Redirect($"/game/{gameName}");

    var isInRoom = await _context.RoomPlayers
        .AnyAsync(p => p.RoomId == roomId 
                    && p.UserId == userProfile.Id
                    && p.Status == PlayerStatus.Active);

    if (!isInRoom)
    {
        return Redirect($"/game/{gameName}");
    }

    ViewBag.GameName = gameName;
    ViewBag.RoomId = roomId;

    return View();
}

    // API ดึงข้อมูล room
    [HttpGet("{roomId}/details")]
    public IActionResult GetRoomById(string gameName, int roomId)
    {
        var currentUserId = _userManager.GetUserId(User);
        var room = _context.Rooms
            .Where(r => r.Id == roomId)
            .Select(r => new RoomDTO
            {
                RoomId = r.Id,
                GameName = r.Game!.GameName,
                RoomName = r.RoomName,
                OwnerUsername = r.RoomOwner!.Nickname,
                GameMode = r.GameMode,
                IsOwner = r.RoomOwner!.UserId == currentUserId,

                RoomSetting = r.RoomSetting == null ? null : new RoomSettingDTO
                {
                    MinRank = r.RoomSetting.MinRank,
                    MaxRank = r.RoomSetting.MaxRank,
                    AllowDuplicateRole = r.RoomSetting.AllowDuplicateRole,
                    IsPrivate = r.RoomSetting.IsPrivate,
                    MaxPlayer = r.RoomSetting.MaxPlayer
                },

                Players = r.Players
                    .Where(p => p.Status == PlayerStatus.Active)
                    .Select(p => new PlayerDTO
                    {
                        UserId = p.UserId,
                        Username = p.User != null ? p.User.Nickname : "",
                        RoleName = p.Role != null ? p.Role.RoleName : "",
                        RankName = p.Rank != null ? p.Rank.RankImageUrl : "",
                        UserProfile = p.User != null ? p.User.ProfileImagePath ?? "" : ""
                    })
                    .ToList()
            })
            .FirstOrDefault();

        if (room == null)
            return NotFound();

        return Ok(room);
    }

[HttpPut("{roomId}/start")]
public async Task<IActionResult> StartRoom(int roomId)
{
    var room = await _context.Rooms
        .Include(r => r.Players)
        .FirstOrDefaultAsync(r => r.Id == roomId);

    if (room == null)
        return NotFound("Room not found");

    bool roomReady = true;

    foreach (var player in room.Players)
    {
        if (!player.IsReady)
        {
            roomReady = false;
            break;
        }
    }

    if (!roomReady)
        return BadRequest("Not all players are ready");

    room.Status = RoomStatus.Starting;

    await _context.SaveChangesAsync();

    return Ok(new { message = "Room started" });
}

[HttpPut("{roomId}/leave")]
public async Task<IActionResult> LeaveRoom(int roomId)
{
    var currentUser = await _userManager.GetUserAsync(User);

    if (currentUser == null)
        return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

    if (userProfile == null)
        return NotFound("User profile not found");

    var player = await _context.RoomPlayers
        .FirstOrDefaultAsync(p => p.RoomId == roomId
                               && p.UserId == userProfile.Id
                               && p.Status == PlayerStatus.Active);

    if (player == null)
        return NotFound("Player not in room");

    player.Status = PlayerStatus.Left;

        var room = await _context.Rooms
        .Include(r => r.Game)
        .FirstOrDefaultAsync(r => r.Id == roomId);


    await _context.SaveChangesAsync();

    await _hub.Clients
        .Group($"room-{roomId}")
        .SendAsync("RoomUpdated", roomId);

    await _hub.Clients.Group(room.Game.GameName)
        .SendAsync("PlayerJoinedRoom", room.Game.GameName);
    return Ok(new { message = "Left room" });
}
    }