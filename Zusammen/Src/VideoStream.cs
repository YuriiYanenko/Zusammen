using QueryFirst;
using System.IO;
using System.Net;

namespace Zusammen;

public class VideoStream
{
    private readonly string _path;

    public VideoStream(string filePath)
    {
        _path = filePath;
    }
    public async void WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
    {
        try
        {
            byte[] buffer = new byte[65536];
            using (var video = File.Open(_path, FileMode.Open, FileAccess.Read))
            {
                var length = (int)video.Length;
                var bytesRead = 1;

                while (length > 0 && bytesRead > 0)
                {
                    bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                    await outputStream.WriteAsync(buffer, 0, bytesRead);
                    length -= bytesRead;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }
        finally
        {
            outputStream.Close();   
        }
    }
}