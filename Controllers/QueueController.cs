using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using hateekub.Hubs;
using hateekub.Services;

namespace hateekub.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    public class QueueController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHubContext<RoomHub> _hub;
        private readonly NotificationService _notificationService;

        public QueueController(
            AppDbContext context,
            UserManager<IdentityUser> userManager,
            IHubContext<RoomHub> hub,
            NotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _hub = hub;
            _notificationService = notificationService;
        }

        // ── GET /api/rooms/{roomId}/queue ─────────────────────────────────────
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
                    queuePlayerId = p.Id,
                    p.User!.Nickname,
                    p.Role!.RoleName,
                    p.Rank!.RankName,
                    p.User!.ProfileImagePath
                })
                .ToListAsync();

            return Ok(queuePlayers);
        }

        // ── PUT /api/rooms/{roomId}/accept/{queuePlayerId} ────────────────────
        [HttpPut("{roomId}/accept/{queuePlayerId}")]
        public async Task<IActionResult> AcceptQueue(int roomId, int queuePlayerId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

            var room = await _context.Rooms
                .Include(r => r.Players)
                .Include(r => r.RoomSetting)
                .Include(r => r.Game)
                .FirstOrDefaultAsync(r => r.Id == roomId);

            if (room == null)
                return NotFound("Room not found");

            if (room.OwnerId != userProfile!.Id)
                return Forbid();

            if (room.ActivePlayers.Count() >= room.RoomSetting!.MaxPlayer)
                return BadRequest("Room is full");

            var queuePlayer = await _context.RoomPlayers
                .FirstOrDefaultAsync(p => 
                    p.Id == queuePlayerId
                    && p.RoomId == roomId
                    && p.Status == PlayerStatus.Queue);

            if (queuePlayer == null)
                return NotFound("Player not in queue");

            queuePlayer.Status = PlayerStatus.Active;
            await _context.SaveChangesAsync();

            // ── สร้าง Notification ให้ผู้เล่นที่ถูก accept ──
            await _notificationService.CreateAsync(
                userProfileId: queuePlayer.UserId,
                message: $"คุณได้รับการตอบรับเข้าห้อง \"{room.RoomName}\"",
                roomId: roomId
            );

            await _hub.Clients.Group($"room-{roomId}").SendAsync("QueueUpdated", roomId);
            await _hub.Clients.Group(room.Game!.GameName).SendAsync("PlayerJoinedRoom", room.Game.GameName);

            return Ok(new { message = "Player accepted" });
        }

        // ── PUT /api/rooms/{roomId}/reject/{queuePlayerId} ────────────────────
        [HttpPut("{roomId}/reject/{queuePlayerId}")]
        public async Task<IActionResult> RejectQueue(int roomId, int queuePlayerId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return Unauthorized();

            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

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
                                       && p.Status == PlayerStatus.Queue);

            if (queuePlayer == null)
                return NotFound("Player not in queue");

            queuePlayer.Status = PlayerStatus.Rejected;
            await _context.SaveChangesAsync();

            // ── สร้าง Notification ให้ผู้เล่นที่ถูก reject ──
            await _notificationService.CreateAsync(
                userProfileId: queuePlayer.UserId,
                message: $"คำขอเข้าห้อง \"{room.RoomName}\" ถูกปฏิเสธ",
                roomId: roomId
            );

            await _hub.Clients.Group($"room-{roomId}").SendAsync("QueueUpdated", roomId);
            await _hub.Clients.Group(room.Game!.GameName).SendAsync("PlayerJoinedRoom", room.Game.GameName);

            return Ok(new { message = "Player rejected" });
        }

        // ── GET /api/rooms/{roomId}/my-queue-status ───────────────────────────
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

        // ── GET /api/rooms/my-queue-rooms ─────────────────────────────────────
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
                    roomId   = p.RoomId,
                    roomName = p.Room!.RoomName,
                    gameName = p.Room!.Game!.GameName,
                    status   = p.Status.ToString()
                })
                .ToListAsync();

            return Ok(rooms);
        }
    }
}