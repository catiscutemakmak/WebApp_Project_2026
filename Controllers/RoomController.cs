using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;
using hateekub.DTOS;
namespace hateekub.Controllers
{
[Route("game/{gameName}/room")]
public class RoomController : Controller
{
    private readonly AppDbContext _context;

    public RoomController(AppDbContext context)
    {
        _context = context;
    }

    [Route("{roomId:int}")]
    public IActionResult Room(string gameName, int roomId)
    {
        ViewBag.GameName = gameName;
        ViewBag.RoomId = roomId;

        return View();
    }
[HttpGet("by-game-name/{gameName}")]
public IActionResult GetRoomsByGameName(string gameName)
{

            var rooms = _context.Rooms
        .Where(r => r.Game!.GameName == gameName)
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
}
}