using hateekub.Data;
using hateekub.Models;
using Microsoft.EntityFrameworkCore;

namespace hateekub.Services
{
    public class NotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        // ─── สร้าง Notification ใหม่ ─────────────────────────────────────────
        public async Task CreateAsync(int userProfileId, string message, int? roomId = null)
        {
            var notification = new Notification
            {
                UserProfileId = userProfileId,
                Message       = message,
                RoomId        = roomId,
                IsRead        = false,
                CreatedAt     = DateTime.UtcNow
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        // ─── ดึง notifications ทั้งหมด ─────────────────────────────────────────
        public async Task<List<Notification>> GetAllNotificationsAsync(int userProfileId, int take = 50)
        {
            return await _context.Notifications
                .Where(n => n.UserProfileId == userProfileId)
                .Include(n => n.Room)
                    .ThenInclude(r => r!.Game)
                .OrderByDescending(n => n.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        // ─── ดึง notifications ที่ยังไม่ได้อ่าน ──────────────────────────────────
        public async Task<List<Notification>> GetUnreadNotificationsAsync(int userProfileId, int take = 20)
        {
            return await _context.Notifications
                .Where(n => n.UserProfileId == userProfileId && !n.IsRead)
                .Include(n => n.Room)
                    .ThenInclude(r => r!.Game)
                .OrderByDescending(n => n.CreatedAt)
                .Take(take)
                .ToListAsync();
        }

        // ─── Mark อ่านแล้ว ─────────────────────────────────────────────────────
        public async Task MarkAsReadAsync(int notificationId, int userProfileId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId
                                       && n.UserProfileId == userProfileId);
            if (notification == null) return;

            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }

        // ─── Mark ทั้งหมดอ่านแล้ว ──────────────────────────────────────────────
        public async Task MarkAllAsReadAsync(int userProfileId)
        {
            var unread = await _context.Notifications
                .Where(n => n.UserProfileId == userProfileId && !n.IsRead)
                .ToListAsync();

            foreach (var n in unread)
                n.IsRead = true;

            await _context.SaveChangesAsync();
        }

        // ─── ลบ notification ───────────────────────────────────────────────────
        public async Task DeleteNotificationAsync(int notificationId, int userProfileId)
        {
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId
                                       && n.UserProfileId == userProfileId);
            if (notification == null) return;

            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
        }
    }
}