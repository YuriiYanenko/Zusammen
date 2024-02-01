using Microsoft.AspNetCore.SignalR;

namespace Zusammen.Hubs;

public class ChatHub:Hub
{
    public async void SendMessage(int roomId, string user, string message)
    {
        await Clients.Group(roomId.ToString()).SendAsync("RecieveMessage", user, message);
    } 
    
    public async Task JoinRoom(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }
}