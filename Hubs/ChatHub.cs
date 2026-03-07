using Microsoft.AspNetCore.SignalR;

namespace hateekub.Hubs
{
    public class ChatHub : Hub
    {
        // ให้ user join room
        public async Task JoinRoom(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        // ส่งข้อความไปยังทุกคนใน room
        public async Task SendMessage(string roomId, string username, string avatar, string message)
        {
            await Clients.Group(roomId).SendAsync("ReceiveMessage", username, avatar, message);
        }
    }
}