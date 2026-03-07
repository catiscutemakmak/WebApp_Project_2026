using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;
using hateekub.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using hateekub.Hubs;


namespace hateekub.Controllers
{
[Route("game/{gameName}")]
public class MatchController : Controller
{
    private readonly AppDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;

    private readonly IHubContext<RoomHub> _hub;
    
    public MatchController(AppDbContext context, UserManager<IdentityUser> userManager,IHubContext<RoomHub> hub)
    {
        _context = context;
        _userManager = userManager;
        _hub = hub;
    }

[HttpGet("")]
public IActionResult Match(string gameName)
        {
            ViewBag.GameName = gameName;
            return View();
        }
[HttpGet("rooms")]


public IActionResult GetRoomsByGameName(string gameName)
{
    var rooms = _context.Rooms
        .Where(r => r.Game != null && r.Game.GameName == gameName)
        .Select(r => new RoomDTO
        {
            RoomId = r.Id,
            GameName = r.Game!.GameName,
            RoomName = r.RoomName,
            OwnerUsername = r.RoomOwner!.UserId,
            GameMode = r.GameMode,

            RoomSetting = r.RoomSetting == null ? null : new RoomSettingDTO
            {
                MinRank = r.RoomSetting!.MinRank,
                MaxRank = r.RoomSetting!.MaxRank,
                AllowDuplicateRole = r.RoomSetting.AllowDuplicateRole,
                IsPrivate = r.RoomSetting.IsPrivate,
                MaxPlayer = r.RoomSetting.MaxPlayer
            },

            Players = r.Players
                .Where(p => !p.IsInQueue)
                .Select(p => new PlayerDTO
                {
                    UserId = p.UserId,
                    Username = p.User != null ? p.User.Nickname : "",
                    RoleName = p.Role != null ? p.Role.RoleName : "",
                    RankName = p.Rank != null ? p.Rank.RankImageUrl : "",
                    UserProfile = p.User != null ? p.User.ProfileImagePath : "",
                })
                .ToList()
        })
        .ToList();

    return Ok(rooms);
}

[HttpGet("roles")]
public IActionResult GetGameRoleByGameName(string gameName)
    {
        
        var game = _context.Games.FirstOrDefault(g => g.GameName.ToLower() == gameName.ToLower());
        if (game == null)
        {
            return NotFound();
        }

        var roles = _context.GameRoles
            .Where(r => r.GameId == game.Id)
            .Select(r => new
            {

                RoleId = r.Id,
                RoleName = r.RoleName,
            })
            .ToList();

        return Ok(roles);

    }
[HttpPost("JoinRoom/{roomId}")]
public async Task<IActionResult> JoinRoom(int roomId, [FromBody] JoinRoomRequest request)
{
    if (string.IsNullOrEmpty(request.RoleName))
        return BadRequest("Role is required");

    var room = await _context.Rooms
        .Include(r => r.Players)
        .Include(r => r.RoomSetting)
        .Include(r => r.Game)
        .FirstOrDefaultAsync(r => r.Id == roomId);

    

    if (room == null)
        return NotFound("Room not found");

    var currentUser = await _userManager.GetUserAsync(User);
    if (currentUser == null)
        return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

    if (userProfile == null)
        return BadRequest("User profile not found");

    // กัน join ซ้ำ
    var alreadyInRoom = room.Players
        .Any(p => p.UserId == userProfile.Id);

    if (alreadyInRoom)
        return BadRequest("You already joined this room");

    var isPrivate = room.RoomSetting!.IsPrivate;

    // นับเฉพาะคนที่ join จริง
    var activePlayerCount = room.Players
        .Count(p => !p.IsInQueue);

    if (!isPrivate && activePlayerCount >= room.RoomSetting.MaxPlayer)
        return BadRequest("Room is full");

    var gameRole = await _context.GameRoles
        .FirstOrDefaultAsync(gr =>
            gr.RoleName == request.RoleName &&
            gr.GameId == room.GameId);

    if (gameRole == null)
        return BadRequest("Invalid role");

    var rankId = await _context.GameRanks
    .Where(p => p.GameId == room.GameId)
    .Select(p => p.Id)
    .FirstOrDefaultAsync();


    var newPlayer = new RoomPlayer
    {
        UserId = userProfile.Id,
        RoomId = room.Id,
        RoleId = gameRole.Id,
        RankId = rankId,
        JoinedAt = DateTime.UtcNow,
        IsReady = false,
        IsInQueue = isPrivate
    };

    room.Players.Add(newPlayer);

    await _context.SaveChangesAsync();
    await _hub.Clients.Group(room.Game.GameName)
        .SendAsync("PlayerJoinedRoom", room.Game.GameName);
    
    return Ok(new
    {
        success = true,
        roomId = room.Id,
        roomName = room.RoomName,
        gameName = room.Game!.GameName,
        roomUrl = isPrivate
            ? null
            : $"/game/{room.Game!.GameName}/room/{room.Id}",
        message = isPrivate
            ? "Added to queue"
            : "Joined room"
    });
}
}
}