using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace Zusammen.Controllers;

public class StreamController : ApiController
{
    [System.Web.Http.HttpGet]
    public HttpContent Get(string filePath)  
    {  
        var video = new VideoStream(filePath);

        var response = Request.CreateResponse();  
        response.Content = new PushStreamContent(video.WriteToStream, new MediaTypeHeaderValue("video/mp4"));
        
        return response.Content;
    }      
}