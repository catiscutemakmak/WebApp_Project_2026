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

    // =========================
    // VIEW PROFILE
    // =========================
    public async Task<IActionResult> ViewProfile(int? id)
    {
        UserProfile? profile;

        if (id.HasValue)
        {
            profile = await _context.UserProfiles
                .Include(p => p.ProfileGames)   // 🔥 เพิ่มบรรทัดนี้
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
                .Include(p => p.ProfileGames)   // 🔥 เพิ่มบรรทัดนี้
                .Include(p => p.Reviews)
                    .ThenInclude(r => r.Reviewer)
                .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

            if (profile == null)
            {
                profile = new UserProfile
                {
                    UserId = currentUser.Id,
                    Username = currentUser.UserName ?? ""
                };

                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
            }
        }

        if (profile == null)
            return NotFound();

        var currentUserForCheck = await _userManager.GetUserAsync(User);
        ViewBag.IsOwnProfile =
            currentUserForCheck != null &&
            profile.UserId == currentUserForCheck.Id;

        return View(profile);
    }

    // =========================
    // GET EDIT PROFILE
    // =========================
    [HttpGet]
    public async Task<IActionResult> EditProfile()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return RedirectToAction("Login", "Account");

        var profile = await _context.UserProfiles
            .Include(p => p.ProfileGames)
            .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

        if (profile == null)
        {
            profile = new UserProfile
            {
                UserId = currentUser.Id,
                Username = currentUser.UserName ?? ""
            };

            _context.UserProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        if (profile.ProfileGames.Count < 3)
        {
            for (int i = profile.ProfileGames.Count; i < 3; i++)
            {
                profile.ProfileGames.Add(new ProfileGame());
            }
        }

        return View(profile);
    }

    // =========================
    // POST EDIT PROFILE
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(UserProfile model, IFormFile? ProfileImage)
    {
        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser == null)
            return RedirectToAction("Login", "Account");

        var profile = await _context.UserProfiles
            .Include(p => p.ProfileGames)
            .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

        if (profile == null)
            return NotFound();

if (ProfileImage != null && ProfileImage.Length > 0)
{
    var uploadsFolder = Path.Combine(
        Directory.GetCurrentDirectory(),
        "wwwroot/images/profile");

    if (!Directory.Exists(uploadsFolder))
        Directory.CreateDirectory(uploadsFolder);

    var extension = Path.GetExtension(ProfileImage.FileName);
    var fileName = $"{profile.Id}.jpg";
    var filePath = Path.Combine(uploadsFolder, fileName);

    // ลบรูปเก่าทุกไฟล์ที่ขึ้นต้นด้วย profile.Id
    var oldFiles = Directory.GetFiles(uploadsFolder, $"{profile.Id}.*");
    foreach (var file in oldFiles)
    {
        System.IO.File.Delete(file);
    }

    using (var fileStream = new FileStream(filePath, FileMode.Create))
    {
        await ProfileImage.CopyToAsync(fileStream);
    }

    profile.ProfileImagePath = "/images/profile/" + fileName;
}
        // ====== Update Basic Info ======
        profile.Nickname = model.Nickname;
        profile.PhoneNumber = model.PhoneNumber;
        profile.Description = model.Description;

        // ====== Update Games ======
        profile.ProfileGames.Clear();   // ลบของเก่าออกก่อน

        if (model.ProfileGames != null)
        {
            foreach (var game in model.ProfileGames)
            {
                if (!string.IsNullOrEmpty(game.GameName))
                {
                    profile.ProfileGames.Add(new ProfileGame
                    {
                        GameName = game.GameName,
                        Rank = game.Rank
                    });
                }
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("ViewProfile");
    }

        public IActionResult View(int id)
        {
            var user = _context.UserProfiles
                .Include(x => x.ProfileGames)
                .Include(x => x.Reviews)
                .ThenInclude(r => r.Reviewer)
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
                return NotFound();

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            ViewBag.IsOwnProfile = user.UserId == currentUserId;

            return View("ViewProfile", user);
        }
}