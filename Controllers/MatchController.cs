using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;
using hateekub.DTOS;
using Microsoft.AspNetCore.Identity;
namespace hateekub.Controllers
{
[Route("game/{gameName}")]
public class MatchController : Controller
{
    private readonly AppDbContext _context;

    private readonly UserManager<IdentityUser> _userManager;
    public MatchController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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

// [HttpPost("JoinRoom/{roomId}")]
// public async Task<IActionResult> JoinRoom(int roomId, string roleName)
// {
//     var room = _context.Rooms.FirstOrDefault(r => r.Id == roomId);
//     if (room == null)
//     {
//         return NotFound();
//     }
//     var roomSetting = _context.RoomSettings.FirstOrDefault(rs => rs.RoomId == roomId);
//     var currentUser = await _userManager.GetUserAsync(User);

//     if (currentUser == null)
//     {
//         return RedirectToAction("Login", "Account");
//     }

//     var alreadyInRoom = room.Players
//         .Any(p => p.UserId == currentUser.Id);

//     if (alreadyInRoom)
//     {
//         return BadRequest("You already joined this room.");
//     }

//     if (room.Players.Count >= room.RoomSetting!.MaxPlayer)
//         return BadRequest("Room is full.");

//     var gameRole = _context.GameRoles.FirstOrDefault(gr => gr.RoleName == roleName && gr.GameId == room.GameId);

//     if (gameRole == null)
//     {
//         return BadRequest("Invalid role");
//     }

//     room.Players.Add(new RoomPlayer
//     {
//         UserId = currentUser.Id,
//         RoomId = room.Id,
//         RoleId = gameRole.Id,
//         RankId = 1, // Default rank, you can modify this as needed
//         JoinedAt = DateTime.UtcNow,
//         IsReady = false
//     });
//     _context.SaveChanges();
//     return Ok(room);
// }
}
}