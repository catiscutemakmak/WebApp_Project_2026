using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using hateekub.Models;
using hateekub.Data;
using hateekub.DTOS;

namespace hateekub.Controllers;

public class NoticeController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public NoticeController(AppDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Notice()
    {
        // ดึง Identity User ที่ login อยู่
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null)
        {
            return Unauthorized();
        }

        // หา UserProfile จาก UserId
        var userProfile = await _db.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == identityUser.Id);

        if (userProfile == null)
        {
            return NotFound("UserProfile not found.");
        }

        // ดึง Notifications ของ user นี้ พร้อมข้อมูล Room, Game และ ActorUser และแปลงเป็น DTO
        var notifications = await _db.Notifications
            .Where(n => n.UserProfileId == userProfile.Id)
            .Include(n => n.Room)
                .ThenInclude(r => r!.Game)
            .Include(n => n.ActorUser)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationDTO
            {
                Id = n.Id,
                Message = n.Message,
                RoomId = n.RoomId,
                RoomName = n.Room != null ? n.Room.RoomName : null,
                GameName = n.Room != null && n.Room.Game != null ? n.Room.Game.GameName : null,
                ActorUserName = n.ActorUser != null ? n.ActorUser.Nickname : null,
                ActorProfileImage = n.ActorUser != null ? n.ActorUser.ProfileImagePath : null,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            })
            .ToListAsync();

        return View(notifications);
    }

    [HttpPost]
    [Route("notice/{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
    {
        var notification = await _db.Notifications
            .Include(n => n.Room)
                .ThenInclude(r => r!.Game)
            .FirstOrDefaultAsync(n => n.Id == notificationId);
            
        if (notification == null)
        {
            return NotFound();
        }

        // ตรวจสอบว่า notification นี้เป็นของ user ที่ login อยู่
        var identityUser = await _userManager.GetUserAsync(User);
        var userProfile = await _db.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == identityUser!.Id);

        if (notification.UserProfileId != userProfile!.Id)
        {
            return Unauthorized();
        }

        notification.IsRead = true;
        await _db.SaveChangesAsync();

        // ถ้ามี Room ให้ redirect ไปหน้า match
        if (notification.Room != null)
        {
            var gameName = notification.Room.Game?.GameName;
            if (!string.IsNullOrEmpty(gameName))
            {
                return Redirect($"/game/{Uri.EscapeDataString(gameName)}/room/{notification.Room.Id}");
            }
        }

        return RedirectToAction("Notice");
    }

    [HttpPost]
    [Route("notice/{notificationId}/delete")]
    public async Task<IActionResult> DeleteNotification(int notificationId)
    {
        var notification = await _db.Notifications.FindAsync(notificationId);
        if (notification == null)
        {
            return NotFound();
        }

        // ตรวจสอบว่า notification นี้เป็นของ user ที่ login อยู่
        var identityUser = await _userManager.GetUserAsync(User);
        var userProfile = await _db.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == identityUser!.Id);

        if (notification.UserProfileId != userProfile!.Id)
        {
            return Unauthorized();
        }

        _db.Notifications.Remove(notification);
        await _db.SaveChangesAsync();

        return RedirectToAction("Notice");
    }

    [HttpGet]
    [Route("notice/unread-count")]
    public async Task<IActionResult> GetUnreadCount()
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null)
        {
            return Unauthorized();
        }

        var userProfile = await _db.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == identityUser.Id);

        if (userProfile == null)
        {
            return NotFound("UserProfile not found.");
        }

        var unreadCount = await _db.Notifications
            .Where(n => n.UserProfileId == userProfile.Id && !n.IsRead)
            .CountAsync();

        return Ok(new { count = unreadCount });
    }  
    [HttpPost]
    [Route("notice/mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null)
        {
            return Unauthorized();
        }

        var userProfile = await _db.UserProfiles
            .FirstOrDefaultAsync(u => u.UserId == identityUser.Id);

        if (userProfile == null)
        {
            return NotFound("UserProfile not found.");
        }

        var unreadNotifications = await _db.Notifications
            .Where(n => n.UserProfileId == userProfile.Id && !n.IsRead)
            .ToListAsync();

        foreach (var notification in unreadNotifications)
        {
            notification.IsRead = true;
        }

        await _db.SaveChangesAsync();

        return RedirectToAction("Notice");
    }
}