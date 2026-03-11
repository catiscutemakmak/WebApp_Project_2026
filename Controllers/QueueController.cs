using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using hateekub.Data;
using hateekub.Models;
using hateekub.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using hateekub.Hubs;
using Microsoft.AspNetCore.Http.HttpResults;


namespace hateekub.Controllers
{


    [ApiController]
    [Route("api/rooms")]
    public class QueueController : Controller
    {
        
    private readonly AppDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;

    private readonly IHubContext<RoomHub> _hub;
    

        public QueueController(AppDbContext context, UserManager<IdentityUser> userManager,IHubContext<RoomHub> hub)
    {
        _context = context;
        _userManager = userManager;
        _hub = hub;
    }

[HttpGet("{roomId}/queue")]
public async Task<IActionResult> GetQueueInRoom(int roomId)
{   
    var roomExists = await _context.Rooms.AnyAsync(r => r.Id == roomId);

    if (!roomExists)
    return NotFound("Room not found");
    var queuePlayers = await _context.RoomPlayers
        .Where(p => p.RoomId == roomId && p.Status == PlayerStatus.Queue)
        .Select(p => new
        {
            p.User!.Nickname,
            p.Role!.RoleName,
            p.Rank!.RankName,
            p.User!.ProfileImagePath,
            p.User!.Id
        })
        .ToListAsync();

    return Ok(queuePlayers);
}
[HttpPut("{roomId}/accept/{queuePlayerId}")]
public async Task<IActionResult> AcceptQueue(int roomId, int queuePlayerId)
{
    var currentUser = await _userManager.GetUserAsync(User);

    if (currentUser == null)
        return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);
    // หา room
    var room = await _context.Rooms
        .Include(r => r.Players)
        .Include(r=> r.OwnerId)
        .Include(r => r.RoomSetting)
        .Include(r => r.Game)
        .FirstOrDefaultAsync(r => r.Id == roomId);

    if (room == null)
        return NotFound("Room not found");


    if (room.OwnerId != userProfile!.Id)
        return Forbid();

    if(room.ActivePlayers.Count() >= room.RoomSetting!.MaxPlayer)
    {
    return BadRequest("Room is full");
    }

    var queuePlayer = await _context.RoomPlayers
        .FirstOrDefaultAsync(p => p.RoomId == roomId 
                               && p.UserId == queuePlayerId 
                               &&  p.Status == PlayerStatus.Queue);

    if (queuePlayer == null)
        return NotFound("Player not in queue");

    queuePlayer.Status = PlayerStatus.Active;

    // อัปเดต RoomStatus หลัง accept
    var newActiveCount = room.ActivePlayers.Count();
    room.Status = newActiveCount >= room.RoomSetting!.MaxPlayer
        ? RoomStatus.Full
        : RoomStatus.Waiting;

    await _context.SaveChangesAsync();

    // สร้าง notification แจ้ง player ที่ถูกรับเข้าห้อง
    var notification = new Notification
    {
        UserProfileId = queuePlayerId,
        RoomId = roomId,
        ActorUserId = userProfile!.Id,
        Message = $"Your request to join room '{room.RoomName}' has been accepted by the owner",
        CreatedAt = DateTime.UtcNow,
        IsRead = false
    };
    _context.Notifications.Add(notification);

    await _context.SaveChangesAsync();

    await _hub.Clients
    .Group($"room-{roomId}")
    .SendAsync("QueueUpdated",roomId);

    await _hub.Clients.Group(room.Game.GameName)
        .SendAsync("PlayerJoinedRoom", room.Game.GameName);

    return Ok(new { message = "Player accepted" });
}
[HttpPut("{roomId}/reject/{queuePlayerId}")]
public async Task<IActionResult> RejectQueue(int roomId, int queuePlayerId)
{
    var currentUser = await _userManager.GetUserAsync(User);

    if (currentUser == null)
        return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);
    // หา room
    var room = await _context.Rooms
        .Include(r => r.Players)
        .Include(r => r.RoomSetting)
        .Include(r => r.Game)
        .FirstOrDefaultAsync(r => r.Id == roomId);

    if (room == null)
        return NotFound("Room not found");


    if (room.OwnerId != userProfile!.Id)
        return Forbid();

    var queuePlayer = await _context.RoomPlayers
        .FirstOrDefaultAsync(p => p.RoomId == roomId 
                               && p.UserId == queuePlayerId 
                               &&  p.Status == PlayerStatus.Queue);

    if (queuePlayer == null)
        return NotFound("Player not in queue");

    queuePlayer.Status = PlayerStatus.Rejected;

    await _context.SaveChangesAsync();

    // สร้าง notification แจ้ง player ที่ถูกปฏิเสธ
    var notification = new Notification
    {
        UserProfileId = queuePlayerId,
        RoomId = roomId,
        ActorUserId = userProfile!.Id,
        Message = $"Your request to join room '{room.RoomName}' has been rejected by the owner",
        CreatedAt = DateTime.UtcNow,
        IsRead = false
    };
    _context.Notifications.Add(notification);

    await _context.SaveChangesAsync();

    await _hub.Clients
    .Group($"room-{roomId}")
    .SendAsync("QueueUpdated",roomId);

    await _hub.Clients.Group(room.Game.GameName)
        .SendAsync("PlayerJoinedRoom", room.Game.GameName);

    return Ok(new { message = "Player rejected" });
}

[HttpGet("{roomId}/my-queue-status")]
public async Task<IActionResult> MyQueueStatus(int roomId)
{
    var currentUser = await _userManager.GetUserAsync(User);
    if (currentUser == null) return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);
    if (userProfile == null) return NotFound();

    var player = await _context.RoomPlayers
        .Include(p => p.Room!).ThenInclude(r => r!.Game)
        .FirstOrDefaultAsync(p => p.RoomId == roomId && p.UserId == userProfile.Id);

    if (player == null)
        return Ok(new { status = "NotFound" });

    var roomUrl = player.Status == PlayerStatus.Active
        ? $"/game/{player.Room!.Game!.GameName}/room/{roomId}"
        : (string?)null;

    return Ok(new { status = player.Status.ToString(), roomUrl });
}

[Authorize]
[HttpDelete("{roomId}/cancel-queue")]
public async Task<IActionResult> CancelQueue(int roomId)
{
    var currentUser = await _userManager.GetUserAsync(User);
    if (currentUser == null) return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);
    if (userProfile == null) return NotFound();

    var queuePlayer = await _context.RoomPlayers
        .FirstOrDefaultAsync(p => p.RoomId == roomId
                               && p.UserId == userProfile.Id
                               && (p.Status == PlayerStatus.Queue || p.Status == PlayerStatus.Rejected));

    if (queuePlayer == null)
        return NotFound("Queue entry not found");

    _context.RoomPlayers.Remove(queuePlayer);
    await _context.SaveChangesAsync();

    return Ok(new { message = "Queue cancelled" });
}

[HttpGet("my-queue-rooms")]
public async Task<IActionResult> MyQueueRooms()
{
    var currentUser = await _userManager.GetUserAsync(User);
    if (currentUser == null) return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);
    if (userProfile == null) return Ok(new List<object>());

    var rooms = await _context.RoomPlayers
        .Where(p => p.UserId == userProfile.Id
                 && (p.Status == PlayerStatus.Queue || p.Status == PlayerStatus.Rejected))
        .Include(p => p.Room!).ThenInclude(r => r!.Game)
        .Include(p => p.Room!).ThenInclude(r => r!.RoomSetting)
        .Where(p => p.Room != null
                 && p.Room.Status != RoomStatus.Delete
                 && p.Room.Status != RoomStatus.Close
                 && p.Room.RoomSetting != null
                 && p.Room.RoomSetting.IsPrivate)
        .Select(p => new
        {
            roomId = p.RoomId,
            roomName = p.Room!.RoomName,
            gameName = p.Room!.Game!.GameName,
            status = p.Status.ToString()
        })
        .ToListAsync();

    return Ok(rooms);
}
}
}