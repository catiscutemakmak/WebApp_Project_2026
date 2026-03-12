using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;
using hateekub.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;


namespace hateekub.Controllers
{


    [ApiController]
    [Route("api/get")]
    public class RankController : Controller
    {
        
    private readonly AppDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;


    

        public RankController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
      
    }
[HttpGet("rank")]
public async Task<IActionResult> GetGameRanks()
{
    var ranks = await _context.GameRanks
        .Select(r => new
        {
            r.Id,
            r.Game.GameName,
            r.RankName,
            r.RankImageUrl
        })
        .ToListAsync();

    var result = ranks
        .GroupBy(r => r.GameName)
        .ToDictionary(
            g => g.Key,
            g => g.Select(r => new {
                r.Id,
                r.RankName,
                r.RankImageUrl
            })
        );

    return Ok(result);
}

[HttpGet("rank/{gameName}")]
public async Task<IActionResult> GetGameRanksbyGameName(string gameName)
{
    var ranks = await _context.GameRanks
        .Where(r => r.Game.GameName == gameName)
        .Select(r => new
        {
            r.Id,
            r.RankName,
            r.RankImageUrl
        })
        .ToListAsync();

    return Ok(ranks);
}
}
}