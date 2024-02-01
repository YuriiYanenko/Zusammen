using Microsoft.AspNetCore.SignalR;

namespace Zusammen.Hubs;

public class ChatHub:Hub
{
    public async void SendMessage(int roomId,string user, string message)
    {
        await Clients.Group(roomId.ToString()).SendAsync("RecieveMessage", user, message);
    } 
}