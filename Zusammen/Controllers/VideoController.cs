using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Components.Endpoints;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Zusammen.Models;
using System.IO;
using Microsoft.SqlServer.Management.HadrModel;
using Zusammen;
using Zusammen.Models;

namespace Zusammen.Controllers;

public class VideoController : Controller
{
    //Database context.
    private ZusammenDbContext _dbContext;

    public VideoController(ZusammenDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    // Returns new View of room page.
    [HttpGet]
    public async Task<IActionResult> Room(int roomId)
    {
        var dbController = new ZusammenDbController(_dbContext);
        
        var activeRoom = new RoomAndFilm();
        
        // Getting room by id. 
        var room = await dbController.OpenRoomById(roomId);
        // Getting path to film video from room.
        var filmPath = await dbController.GetFilmById(room.Value.film_id);
        
        activeRoom.Room = room.Value;
        activeRoom.FilmPath = filmPath.Value.video_path;
        
        return View(activeRoom);
    }
    
    public string GenerateRoomConnectUrl(int roomId)
    {
        return $"http://localhost:5133/Video/Room?roomId={roomId}";
    }
}