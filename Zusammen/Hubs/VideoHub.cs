using Microsoft.AspNetCore.SignalR;
using MySql.Data.MySqlClient.Memcached;

namespace Zusammen.Hubs;

public class VideoHub : Hub
{
    // Список кімнат та закріплених за ними відео.
    private static Dictionary<int, string> roomVideos = new Dictionary<int, string>();

    // Метод для відправки відео всім учасникам конкретної кімнати.
    public async void SendVideo(int roomId, string videoPath)
    {
        roomVideos[roomId] = videoPath;
        await Clients.Group(roomId.ToString()).SendAsync("ReceiveVideo", videoPath);
    }

    // Метод приєднання учасника до кімнати.
    public async Task JoinRoom(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    // Метод покидання кімнати учасником.
    public async Task LeaveRoom(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
    }

    // Сповіщення всіх підписників групи що відео поставлено на паузу
    public async Task StopVideo(int roomId)
    {
        await Clients.Group(roomId.ToString()).SendAsync("Stop", roomId);
    }

    // Сповіщення всіх підписників групи що відео відтворюється
    public async Task PlayVideo(int roomId)
    {
        await Clients.Group(roomId.ToString()).SendAsync("Play", roomId);
    }

    public async Task SeekVideo(int roomId, int newPosition)
    {
        await Clients.Group(roomId.ToString()).SendAsync("Seek", roomId, newPosition);
    }
    
}