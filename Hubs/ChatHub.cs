using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using hateekub.Data;
using hateekub.Models;

namespace hateekub.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ChatHub(
            AppDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ให้ user join ห้อง
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        // ส่งข้อความ
        public async Task SendMessage(string roomId, string message)
        {
            // ดึง user จาก Identity
            var user = await _userManager.GetUserAsync(Context.User);

            if (user == null)
            {
                await Clients.Group(roomId)
                    .SendAsync("ReceiveMessage", "Unknown", "/images/default-avatar.png", message);
                return;
            }

            // ดึงข้อมูล profile
            var profile = await _context.UserProfiles
                .FirstOrDefaultAsync(p => p.UserId == user.Id);

            var username = profile?.Nickname ?? user.UserName ?? "Unknown";
            var avatar = profile?.ProfileImagePath ?? "/images/default-avatar.png";

             //(save chat)
            var chat = new RoomChat
            {
                RoomId = int.Parse(roomId),
                UserId = user.Id,
                Message = message,
                SentAt = DateTime.UtcNow
            };

                _context.RoomChats.Add(chat);
                await _context.SaveChangesAsync();
        
                await Clients.Group(roomId)
                .SendAsync("ReceiveMessage", username, avatar, message);
        }
    }
}