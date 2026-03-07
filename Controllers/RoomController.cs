using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using hateekub.Data;
using hateekub.Models;
using hateekub.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using hateekub.Hubs;
using Microsoft.EntityFrameworkCore;

[Route("game/{gameName}/room")]
public class RoomController : Controller
{   
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;


    public RoomController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // หน้า View ของ Room
    [HttpGet("{roomId}")]
    public IActionResult Room(string gameName, int roomId)
    {
        ViewBag.GameName = gameName;
        ViewBag.RoomId = roomId;
        return View();
    }
    [HttpGet("testroom")]
    public IActionResult testroom()
    {

        return View();
    }

    // API ดึงข้อมูล room
    [HttpGet("{roomId}/details")]
    public IActionResult GetRoomById(string gameName, int roomId)
    {
        var currentUserId = _userManager.GetUserId(User);
        var room = _context.Rooms
            .Where(r => r.Id == roomId)
            .Select(r => new RoomDTO
            {
                RoomId = r.Id,
                GameName = r.Game!.GameName,
                RoomName = r.RoomName,
                OwnerUsername = r.RoomOwner!.Nickname,
                GameMode = r.GameMode,
                IsOwner = r.RoomOwner!.UserId == currentUserId,

                RoomSetting = r.RoomSetting == null ? null : new RoomSettingDTO
                {
                    MinRank = r.RoomSetting.MinRank,
                    MaxRank = r.RoomSetting.MaxRank,
                    AllowDuplicateRole = r.RoomSetting.AllowDuplicateRole,
                    IsPrivate = r.RoomSetting.IsPrivate,
                    MaxPlayer = r.RoomSetting.MaxPlayer
                },

                Players = r.Players
                    .Where(p => p.Status == PlayerStatus.Active)
                    .Select(p => new PlayerDTO
                    {
                        UserId = p.UserId,
                        Username = p.User != null ? p.User.Nickname : "",
                        RoleName = p.Role != null ? p.Role.RoleName : "",
                        RankName = p.Rank != null ? p.Rank.RankImageUrl : "",
                        UserProfile = p.User != null ? p.User.ProfileImagePath ?? "" : ""
                    })
                    .ToList()
            })
            .FirstOrDefault();

        if (room == null)
            return NotFound();

        return Ok(room);
    }

    }