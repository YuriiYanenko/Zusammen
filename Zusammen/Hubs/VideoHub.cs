using Microsoft.AspNetCore.SignalR;

namespace Zusammen.Hubs;

public class VideoHub : Hub
{
    private static Dictionary<int, string> roomVideos = new Dictionary<int, string>();

    public async void SendVideo(int roomId, string videoPath)
    {
        roomVideos[roomId] = videoPath;
        await Clients.Group(roomId.ToString()).SendAsync("ReceiveVideo", videoPath);
    }

    public async Task JoinRoom(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    public async Task LeaveRoom(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }
}