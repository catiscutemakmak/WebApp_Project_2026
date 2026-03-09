using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using hateekub.Data;
using hateekub.Models;

namespace hateekub.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            var userToReview = await _context.UserProfiles
                .FirstOrDefaultAsync(u => u.Id == id);

            if (userToReview == null)
                return NotFound();

            var model = new ReviewViewModel
            {
                UserProfileId = userToReview.Id,
                Username = userToReview.Username
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Review(ReviewViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var reviewer = await _context.UserProfiles
                .FirstOrDefaultAsync(u => u.UserId == currentUser.Id);

            if (reviewer == null)
                return RedirectToAction("Login", "Account");

            if (reviewer.Id == model.UserProfileId)
            {
                ModelState.AddModelError("", "You cannot review yourself, bruh.");
                return View(model);
            }

            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r =>
                    r.UserProfileId == model.UserProfileId &&
                    r.ReviewerId == reviewer.Id);

            if (existingReview != null)
            {
                existingReview.Description = model.Description;
                existingReview.Rating = model.Rating;
            }
            else
            {
                var review = new Review
                {
                    UserProfileId = model.UserProfileId,
                    ReviewerId = reviewer.Id,
                    Description = model.Description,
                    Rating = model.Rating
                };
                _context.Reviews.Add(review);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ViewProfile", "Profile",
                new { id = model.UserProfileId });
        }

        public async Task<IActionResult> History(int? id)
        {
            UserProfile? profile;

            if (id.HasValue)
            {
                profile = await _context.UserProfiles
                    .FirstOrDefaultAsync(p => p.Id == id.Value);
            }
            else
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser == null)
                    return RedirectToAction("Login", "Account");

                profile = await _context.UserProfiles
                    .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);
            }

            if (profile == null)
                return NotFound();

            var historyEntries = await _context.RoomPlayers
                .Where(rp => rp.UserId == profile.Id
                    && (rp.Status == PlayerStatus.Active || rp.Status == PlayerStatus.Left)
                    && rp.Room!.Status == RoomStatus.Close)
                .Include(rp => rp.Room)
                    .ThenInclude(r => r!.Game)
                .Include(rp => rp.Room)
                    .ThenInclude(r => r!.Players)
                        .ThenInclude(p => p.User)
                .OrderByDescending(rp => rp.Room!.PlayDateTime)
                .ToListAsync();

            ViewBag.ProfileNickname = profile.Nickname ?? profile.Username;
            ViewBag.ProfileId = profile.Id;
            return View(historyEntries);
        }
    }
}