using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;
using hateekub.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using hateekub.Hubs;

namespace hateekub.Controllers;

public class CreateTeamController: Controller
{

    private readonly AppDbContext _context;

    private readonly IHubContext<RoomHub> _hub;
    private readonly UserManager<IdentityUser> _userManager;
    public CreateTeamController(AppDbContext context, UserManager<IdentityUser> userManager, IHubContext<RoomHub> hub)
    {
        _context = context;
        _userManager = userManager;
        _hub = hub;
    }
    public IActionResult Create()
    {
        return View();
    }

[HttpPost]
public async Task<IActionResult> CreateTeam([FromBody] CreateRoomRequest request)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var currentUser = await _userManager.GetUserAsync(User);
    if (currentUser == null)
        return Unauthorized();

    var userProfile = await _context.UserProfiles
        .FirstOrDefaultAsync(p => p.UserId == currentUser.Id);

    if (userProfile == null)
        return BadRequest("User profile not found");

    var game = await _context.Games
        .FirstOrDefaultAsync(g => g.GameName == request.Game);

    if (game == null)
        return BadRequest("Invalid game");

    int? roleId = null;
    int? rankId = null;

    if (request.GameRole != null && request.GameRole != "Any")
    {
        roleId = await _context.GameRoles
            .Where(r => r.RoleName == request.GameRole && r.GameId == game.Id)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();
    }

    if (request.MinRank != null && request.MinRank != "Any")
    {
        rankId = await _context.GameRanks
            .Where(r => r.RankName == request.MinRank && r.GameId == game.Id)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();
    }

    var newRoom = new Room
    {
        RoomName = request.RoomName,
        GameId = game.Id,
        GameMode = request.GameMode,
        Server = request.GameServer,
        OwnerId = userProfile.Id,
        Description = request.Description,
        PlayDateTime = request.PlayDateTime = request.PlayDateTime.ToUniversalTime(),

        RoomSetting = new RoomSetting
        {
            MinRank = request.MinRank,
            MaxRank = request.MaxRank,
            AllowDuplicateRole = false,
            IsPrivate = request.RoomPrivacy,
            MaxPlayer = request.MaxPlayer
        },

        Players = new List<RoomPlayer>
        {
            new RoomPlayer
            {
                UserId = userProfile.Id,
                IsInQueue = false,
                JoinedAt = DateTime.UtcNow,
                IsReady = false,
                RoleId = roleId,
                RankId = rankId
            }
        }
    };

    _context.Rooms.Add(newRoom);
    await _context.SaveChangesAsync();
    await _hub.Clients.All.SendAsync("RoomCreated", game.GameName);
    return Ok(newRoom.Id);
}
}
