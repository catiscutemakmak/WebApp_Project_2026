using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
    await _hub.Clients
    .Group($"room-{roomId}")
    .SendAsync("QueueUpdated",roomId);

    await _hub.Clients.Group(room.Game.GameName)
        .SendAsync("PlayerJoinedRoom", room.Game.GameName);

    return Ok(new { message = "Player rejected" });
}
}
}