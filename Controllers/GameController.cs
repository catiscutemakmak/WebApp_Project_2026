using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;

namespace hateekub.Controllers
{
public class GameController : Controller
{

        private readonly AppDbContext _context;

    public GameController(AppDbContext context)
    {
        _context = context;
    }

[HttpGet("/game/{gameName}/roles")]
public IActionResult GetGameRoleByGameName(string gameName)
    {
        
        var game = _context.Games.FirstOrDefault(g => g.GameName == gameName);
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
}
    }