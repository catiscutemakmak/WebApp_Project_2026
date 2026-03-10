using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hateekub.Data;
using hateekub.Models;
using hateekub.Services;

namespace hateekub.Controllers
{
    [Authorize]                          // ← ต้อง login ก่อนเสมอ
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationService _notificationService;
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public NotificationsController(
            NotificationService notificationService,
            AppDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _notificationService = notificationService;
            _context = context;
            _userManager = userManager;
        }

        // ── helper ────────────────────────────────────────────────────────────
        private async Task<int?> GetCurrentUserProfileIdAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return null;           // ลบ dev fallback ออก

            var userProfile = await _context.UserProfiles
                .FirstOrDefaultAsync(up => up.UserId == currentUser.Id);

            return userProfile?.Id;
        }

        // ── GET api/notifications/all ─────────────────────────────────────────
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllNotifications()
        {
            var userProfileId = await GetCurrentUserProfileIdAsync();
            if (userProfileId == null) return Unauthorized();

            var notifications = await _notificationService.GetAllNotificationsAsync(userProfileId.Value, 50);
            var result = notifications.Select(n => new
            {
                id        = n.Id,
                message   = n.Message,
                isRead    = n.IsRead,
                createdAt = n.CreatedAt,
                gameName  = n.Room?.Game?.GameName ?? "Unknown",
                roomId    = n.RoomId
            });

            return Ok(result);
        }

        // ── GET api/notifications/unread ──────────────────────────────────────
        [HttpGet("unread")]
        public async Task<ActionResult<IEnumerable<object>>> GetUnreadNotifications()
        {
            var userProfileId = await GetCurrentUserProfileIdAsync();
            if (userProfileId == null) return Unauthorized();

            var notifications = await _notificationService.GetUnreadNotificationsAsync(userProfileId.Value, 20);
            var result = notifications.Select(n => new
            {
                id        = n.Id,
                message   = n.Message,
                isRead    = n.IsRead,
                createdAt = n.CreatedAt,
                gameName  = n.Room?.Game?.GameName ?? "Unknown",
                roomId    = n.RoomId
            });

            return Ok(result);
        }

        // ── GET api/notifications/unread-count ────────────────────────────────
        [HttpGet("unread-count")]
        public async Task<ActionResult<int>> GetUnreadCount()
        {
            var userProfileId = await GetCurrentUserProfileIdAsync();
            if (userProfileId == null) return Unauthorized();

            var notifications = await _notificationService.GetUnreadNotificationsAsync(userProfileId.Value, int.MaxValue);
            return Ok(notifications.Count);
        }

        // ── POST api/notifications/{id}/read ──────────────────────────────────
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userProfileId = await GetCurrentUserProfileIdAsync();
            if (userProfileId == null) return Unauthorized();

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return NotFound();
            if (notification.UserProfileId != userProfileId.Value) return Forbid();

            await _notificationService.MarkAsReadAsync(id, userProfileId.Value);
            return Ok();
        }

        // ── DELETE api/notifications/{id} ─────────────────────────────────────
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var userProfileId = await GetCurrentUserProfileIdAsync();
            if (userProfileId == null) return Unauthorized();

            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null) return NotFound();
            if (notification.UserProfileId != userProfileId.Value) return Forbid();

            await _notificationService.DeleteNotificationAsync(id, userProfileId.Value);
            return Ok();
        }

        // ── POST api/notifications/mark-all-read ──────────────────────────────
        [HttpPost("mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userProfileId = await GetCurrentUserProfileIdAsync();
            if (userProfileId == null) return Unauthorized();

            await _notificationService.MarkAllAsReadAsync(userProfileId.Value);
            return Ok();
        }
    }
}