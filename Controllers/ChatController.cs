using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using hateekub.Data;
using hateekub.Models;

[Route("api/chat")]
[ApiController]
public class ChatController : Controller
{
    private readonly AppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ChatController(AppDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage([FromBody] ChatDTO dto)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        var profile = await _context.UserProfiles
            .FirstOrDefaultAsync(p => p.UserId == user.Id);

        var chat = new RoomChat
        {
            RoomId = int.Parse(dto.RoomId),
            UserId = user.Id,
            Message = dto.Message,
            SentAt = DateTime.UtcNow
        };

        _context.RoomChats.Add(chat);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            username = profile?.Nickname ?? user.UserName,
            avatar = profile?.ProfileImagePath ?? "/images/default-avatar.png",
            message = dto.Message
        });
    }

    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetMessages(int roomId)
    {
        var messages = await _context.RoomChats
            .Where(c => c.RoomId == roomId)
            .OrderByDescending(c => c.SentAt)
            .Take(50)
            .Join(
                _context.UserProfiles,
                chat => chat.UserId,
                profile => profile.UserId,
                (chat, profile) => new
                {
                    username = profile.Nickname,
                    avatar = profile.ProfileImagePath ?? "/images/default-avatar.png",
                    message = chat.Message,
                    sentAt = chat.SentAt
                }
            )
            .OrderBy(c => c.sentAt)
            .ToListAsync();

        return Ok(messages);
    }
}