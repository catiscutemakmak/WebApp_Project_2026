using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using hateekub.Data;
using hateekub.Models;

namespace hateekub.Controllers;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ProfileController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> ViewProfile(int? id)
    {
        UserProfile? profile;

        if (id.HasValue)
        {
            profile = await _context.UserProfiles
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.Reviewer)
                .FirstOrDefaultAsync(p => p.Id == id.Value);
        }
        else
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            profile = await _context.UserProfiles
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.Reviewer)
                .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = currentUser.Id,
                    Username = currentUser.UserName ?? "",
                };
                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
            }
        }

        if (profile == null)
            return NotFound();

        var currentUserForCheck = await _userManager.GetUserAsync(User);
        ViewBag.IsOwnProfile = (currentUserForCheck != null && profile.UserId == currentUserForCheck.Id);

        return View(profile);
    }

    public IActionResult EditProfile()
    {
        return View();
    }
}
