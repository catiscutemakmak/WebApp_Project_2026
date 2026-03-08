using Microsoft.AspNetCore.SignalR;

namespace hateekub.Hubs
{

public class RoomHub : Hub
{
    public async Task JoinGameGroup(string gameName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameName);
    }

    public async Task AcceptRejectQueue(string roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId}");
        }
    
}
}

