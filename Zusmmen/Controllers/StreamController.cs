using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MediaTypeHeaderValue = System.Net.Http.Headers.MediaTypeHeaderValue;

namespace Zusmmen.Controllers;

public class StreamController : ApiController
{
    public HttpResponseMessage Get(string filename)  
    {  
        var video = new VideoStream(filename);

        var response = Request.CreateResponse();  
        response.Content = new PushStreamContent(video.WriteToStream, new MediaTypeHeaderValue("video/mp4"));
        return response;  
    }      
}