using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using hateekub.Models;
using hateekub.Data;

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

        // ดึง Notifications ของ user นี้ พร้อมข้อมูล Room และ Game
        var notifications = await _db.Notifications
            .Where(n => n.UserProfileId == userProfile.Id)
            .Include(n => n.Room)
                .ThenInclude(r => r!.Game)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return View(notifications);
    }

    [HttpPost]
    [Route("notice/{notificationId}/read")]
    public async Task<IActionResult> MarkAsRead(int notificationId)
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
}