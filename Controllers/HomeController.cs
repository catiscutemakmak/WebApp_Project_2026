using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using hateekub.Models;
using hateekub.Data;

namespace hateekub.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(AppDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    [Route("game/{gameName}")]
    public async Task<IActionResult> SubmitGame(string gameName, [FromForm] string gameKey, [FromForm] string gameID)
    {
        if (string.IsNullOrWhiteSpace(gameID))
            return BadRequest("Game ID is required.");

        // 1. ดึง Identity User ที่ login อยู่
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null)
            return Unauthorized();

        // 2. หา UserProfile จาก UserId (IdentityUser.Id)
        var userProfile = await _db.UserProfiles
            .Include(u => u.UserGames)
            .FirstOrDefaultAsync(u => u.UserId == identityUser.Id);

        if (userProfile == null)
            return NotFound("UserProfile not found.");

        // 3. หา Game จาก GameName ที่ส่งมา
        var game = await _db.Games.FirstOrDefaultAsync(g => g.GameName == gameName);
        if (game == null)
            return NotFound("Game not found.");

        // 4. เช็คว่าเคย link เกมนี้ไว้แล้วหรือยัง
        var existingUserGame = userProfile.UserGames
            .FirstOrDefault(ug => ug.GameId == game.Id);

        if (existingUserGame == null)
        {
            // ยังไม่เคยกรอก → สร้าง UserGame ใหม่
            var newUserGame = new UserGame
            {
                UserProfileId = userProfile.Id,
                GameId = game.Id,
                InGameName = gameID
            };
            _db.UserGames.Add(newUserGame);
            await _db.SaveChangesAsync();
        }
        // ถ้าเคยกรอกแล้ว → ไม่ทำอะไร ข้ามไป redirect เลย

        return Redirect($"/game/{Uri.EscapeDataString(gameName)}");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}