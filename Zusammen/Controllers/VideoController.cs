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
    [HttpPost]
    public IActionResult Room(int roomId)
    {
        var dbController = new ZusammenDbController(_dbContext);
        
        var activeRoom = new RoomAndFilm();
        
        // Getting room by id. 
        var room = dbController.OpenRoomById(roomId);
        // Getting path to film video from room.
        var filmPath = dbController.GetFilmById(room.Result.Value.film_id);
        
        activeRoom.Room = room.Result.Value;
        activeRoom.FilmPath = filmPath.Result.Value.video_path;
        
        return View(activeRoom);
    }

    // Plays first film from queue.
}