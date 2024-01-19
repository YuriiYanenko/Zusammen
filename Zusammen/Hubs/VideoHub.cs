using Microsoft.AspNetCore.SignalR;

namespace Zusammen.Hubs;

public class VideoHub:Hub
{
    public async void SendVideo(string videoPath)
    {
        await Clients.All.SendAsync("RecieveVideo", videoPath);
    }
}