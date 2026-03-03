using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;
using hateekub.DTOS;
namespace hateekub.Controllers
{
[Route("game/{gameName}")]
public class MatchController : Controller
{
    private readonly AppDbContext _context;

    public MatchController(AppDbContext context)
    {
        _context = context;
    }

        [HttpGet("")]
        public IActionResult Match(string gameName)
        {
            ViewBag.GameName = gameName;
            return View();
        }
[HttpGet("rooms")]
public IActionResult GetRoomsByGameName(string gameName)
{

            var rooms = _context.Rooms
        .Where(r => r.Game != null && r.Game.GameName == gameName)
        .Select(r => new RoomDTO
        {
            RoomId = r.Id,
            GameName = r.Game!.GameName,
            RoomName = r.RoomName,
            OwnerUsername = r.RoomOwner!.Username,
            GameMode = r.GameMode,

            RoomSetting = r.RoomSetting == null ? null : new RoomSettingDTO
            {
                MinRank = r.RoomSetting!.MinRank,
                MaxRank = r.RoomSetting!.MaxRank,
                AllowDuplicateRole = r.RoomSetting.AllowDuplicateRole,
                IsPrivate = r.RoomSetting.IsPrivate,
                MaxPlayer = r.RoomSetting.MaxPlayer
            },

            Players = r.Players
                .Select(p => new PlayerDTO
                {
                    UserId = p.UserId,
                Username = p.User != null ? p.User.Username : "",
                RoleName = p.Role != null ? p.Role.RoleName : "",
                RankName = p.Rank != null ? p.Rank.RankImageUrl : "",
                    UserProfile = null
                })
                .ToList()
        })
        .ToList();

            return Ok(rooms);
}

[HttpGet("roles")]
public IActionResult GetGameRoleByGameName(string gameName)
    {
        
        var game = _context.Games.FirstOrDefault(g => g.GameName.ToLower() == gameName.ToLower());
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