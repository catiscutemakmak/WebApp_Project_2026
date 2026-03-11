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


public async Task<IActionResult> GetRoomsByGameName(string gameName)
{

    var currentUser = await _userManager.GetUserAsync(User);
    if (currentUser == null)
        return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);


var rooms = _context.Rooms
    .AsNoTracking()
    .Where(r => r.Game != null && r.Game.GameName == gameName && (r.Status != RoomStatus.Close && r.Status != RoomStatus.Delete))
    .Select(r => new RoomDTO
    {
        RoomId = r.Id,
        GameName = r.Game!.GameName,
        RoomName = r.RoomName,
        OwnerUsername = r.RoomOwner!.UserId,
        OwnerId = r.RoomOwner!.Id,
        GameMode = r.GameMode,
        RoomStatus = r.Status,

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
                UserProfile = p.User != null ? p.User.ProfileImagePath : "",
                Avatar = p.Avatar, 
            })
            .ToList(),

        MyStatus = r.Players
            .Where(p => p.UserId == userProfile.Id)
            .Select(p => p.Status.ToString())
            .FirstOrDefault()
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
    if (request == null)
        return BadRequest("Invalid request");

    var room = await _context.Rooms
        .Include(r => r.Players)
        .Include(r => r.RoomSetting)
        .Include(r => r.Game)
    
        .FirstOrDefaultAsync(r => r.Id == roomId);

    if (room == null)
        return NotFound("Room not found");

    if (room.Status != RoomStatus.Waiting)
        return BadRequest("Can't Join This Room");

    var currentUser = await _userManager.GetUserAsync(User);
    if (currentUser == null)
        return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

    if (userProfile == null)
        return BadRequest("User profile not found");

    bool isAmongUs = room.Game!.GameName == "Among Us";

    var gameHasRoles = await _context.GameRoles
        .AnyAsync(r => r.GameId == room.GameId);

    // Role required only for games with roles (except Among Us)
    if (gameHasRoles && !isAmongUs && string.IsNullOrEmpty(request.RoleName))
    {
        return BadRequest("Role is required");
    }

    // Avatar required for Among Us
    if (isAmongUs && string.IsNullOrEmpty(request.Avatar))
    {
        return BadRequest("Avatar is required");
    }

    var existingPlayer = room.Players
        .FirstOrDefault(p => p.UserId == userProfile.Id);

    if (existingPlayer != null)
    {
        if (existingPlayer.Status == PlayerStatus.Queue)
            return BadRequest("You are waiting for queue");

        if (existingPlayer.Status == PlayerStatus.Active)
            return BadRequest("You already joined this room");

        if (existingPlayer.Status == PlayerStatus.Kicked)
            return BadRequest("You were kicked from this room");
        
}

    var isPrivate = room.RoomSetting!.IsPrivate;

    var activePlayerCount = room.Players
        .Count(p => p.Status == PlayerStatus.Active);

    if (!isPrivate && activePlayerCount >= room.RoomSetting.MaxPlayer)
        return BadRequest("Room is full");

    GameRole? gameRole = null;

    if (gameHasRoles && !isAmongUs)
    {
        gameRole = await _context.GameRoles
            .FirstOrDefaultAsync(gr =>
                gr.RoleName == request.RoleName &&
                gr.GameId == room.GameId);

        if (gameRole == null)
            return BadRequest("Invalid role");
    }

    // Rank (optional)
    var gameHasRanks = await _context.GameRanks
        .AnyAsync(r => r.GameId == room.GameId);

    int? rankId = null;

    if (gameHasRanks)
    {
        if (request.RankId == null)
            return BadRequest("Rank is required");

        var rank = await _context.GameRanks
            .FirstOrDefaultAsync(r =>
                r.Id == request.RankId &&
                r.GameId == room.GameId);

        if (rank == null)
            return BadRequest("Invalid rank");

        rankId = rank.Id;
    }

    // Prevent duplicate avatar (Among Us)
    if (isAmongUs)
    {
        var avatarTaken = room.Players
            .Any(p => p.Avatar == request.Avatar &&
                      p.Status == PlayerStatus.Active);

        if (avatarTaken)
            return BadRequest("Avatar already taken");
    }

    // ถ้าเคยโดน reject → update record เดิม
    var rejectedRecord = room.Players
        .FirstOrDefault(p => p.UserId == userProfile.Id
                          && p.Status == PlayerStatus.Rejected);

    if (rejectedRecord != null)
    {
        rejectedRecord.RoleId = gameRole?.Id;
        rejectedRecord.Avatar = request.Avatar;
        rejectedRecord.JoinedAt = DateTime.UtcNow;
        rejectedRecord.IsReady = false;
        rejectedRecord.Status = isPrivate
            ? PlayerStatus.Queue
            : PlayerStatus.Active;
    }
    else
    {
        var newPlayer = new RoomPlayer
        {
            UserId = userProfile.Id,
            RoomId = room.Id,
            RoleId = gameRole?.Id,
            RankId = rankId,
            Avatar = request.Avatar,
            JoinedAt = DateTime.UtcNow,
            IsReady = false,
            Status = isPrivate
                ? PlayerStatus.Queue
                : PlayerStatus.Active
        };

        room.Players.Add(newPlayer);
    }

    await _context.SaveChangesAsync();

    // สร้าง notifications
    if (isPrivate)
    {
        // แจ้งเจ้าของห้องว่ามีคนขอ join
        var ownerNotification = new Notification
        {
            UserProfileId = room.OwnerId,
            RoomId = room.Id,
            ActorUserId = userProfile.Id,
            Message = $"{userProfile.Nickname} requested to join your room '{room.RoomName}'",
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };
        _context.Notifications.Add(ownerNotification);

        // แจ้ง user ว่าอยู่ใน queue
        var userNotification = new Notification
        {
            UserProfileId = userProfile.Id,
            RoomId = room.Id,
            ActorUserId = userProfile.Id,
            Message = $"You are in the queue for room '{room.RoomName}'. Waiting for owner's approval.",
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };
        _context.Notifications.Add(userNotification);
    }
    else
    {
        // แจ้งเจ้าของห้องว่ามีคน join สำเร็จ
        var ownerNotification = new Notification
        {
            UserProfileId = room.OwnerId,
            RoomId = room.Id,
            ActorUserId = userProfile.Id,
            Message = $"{userProfile.Nickname} has joined your room '{room.RoomName}'",
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };
        _context.Notifications.Add(ownerNotification);

        // แจ้ง user ว่า join สำเร็จ
        var userNotification = new Notification
        {
            UserProfileId = userProfile.Id,
            RoomId = room.Id,
            ActorUserId = userProfile.Id,
            Message = $"You successfully joined room '{room.RoomName}'",
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };
        _context.Notifications.Add(userNotification);
    }

    await _context.SaveChangesAsync();

    // realtime update rooms list
    await _hub.Clients.Group(room.Game.GameName)
        .SendAsync("PlayerJoinedRoom", room.Game.GameName);

    // queue update
    await _hub.Clients
        .Group($"room-{roomId}")
        .SendAsync("QueueUpdated", roomId);

    return Ok(new
    {
        success = true,
        roomId = room.Id,
        roomName = room.RoomName,
        gameName = room.Game.GameName,
        roomUrl = isPrivate
            ? $"/game/{room.Game.GameName}"
            : $"/game/{room.Game.GameName}/room/{room.Id}",
        message = isPrivate
            ? "Added to queue"
            : "Joined room"
    });
}

}
}
//test//