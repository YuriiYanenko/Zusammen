using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Zusammen.Models;

namespace Zusammen.Controllers;

public class ZusammenDbController : Controller
{
    //Context of Zusammen database (sets of tables objects).
    private readonly ZusammenDbContext _context;

    //Constructor, get db context from localhost:5344/zusammen. 
    public ZusammenDbController(ZusammenDbContext context)
    {
        _context = context;
    }
    
    //Gets List of values from films table Method GET.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<films>>> GetFilms()
    {
        return await _context.films.ToListAsync();
    }

    //Returns film object (row from films table) with specified id attribute. HTTP method GET. 
    [HttpGet("{id}")]
    public async Task<ActionResult<films>> GetFilmById(int id)
    {   
        var film = await _context.films.FindAsync(id);
        var notFoundFilm = new films();
        notFoundFilm.id = 0;
        if (null == film)
        {
            return notFoundFilm;
        }

        return film;
    }

    // Returns list of films with specified string in name. 
    public async Task<ActionResult<List<films>>> GetFilmByName(string name)
    {
        // Get all films where name contains specified value and convert it to list.
        var film = await _context.films.
            Where(e => EF.Functions.Like(e.name.ToLower(), $"%{name.ToLower()}%")).
            ToListAsync();
        if (film.Count == 0)
        {
            return NotFound();
        }

        return film;
    }
    
    // Add row to films table and returns that film object. HTTP method POST.
    [HttpPost] 
    public async Task<IActionResult> AddFilm(films filmToAdd)
    {
        _context.films.Add(filmToAdd);
        await _context.SaveChangesAsync();
        return CreatedAtAction("GetFilmById", new { id = filmToAdd.id }, filmToAdd);
    }

    // Return room with specified id.
    public async Task<ActionResult<rooms>> OpenRoomById(int roomId)
    {
        var room = await _context.rooms.FindAsync(roomId);
        if (null == room)
            return NotFound();
        return room;
    }

    // Метод додавання нової кімнати до бази даних.
    [HttpPost]
    public async Task<IActionResult> CreateRoom(int filmId)
    {
        var film = GetFilmById(filmId).Result.Value;
        var roomList = await _context.rooms.ToListAsync();
        var lastRoom = roomList[roomList.Count - 1];
        var room = new rooms();
        // Автоматичне інкременування id кімнати.
        room.name = film.name;
        room.id = lastRoom.id +1 ;

        room.admin_id = 1;
        room.film_id = filmId;
        room.members_id = new[] { 1 };
        _context.rooms.Add(room);
        await _context.SaveChangesAsync();
        return View("~/Views/Video/Room.cshtml", CreateCombinedTable(room).Result.Value);
        //return CreatedAtAction("OpenRoomById", new {roomId = room.id}, room);
    }

    public async Task<ActionResult<RoomAndFilm>> CreateCombinedTable(rooms room)
    {
        RoomAndFilm roomAndFilm = new RoomAndFilm();
        var filmPath = GetFilmById(room.film_id).Result.Value.video_path;
        roomAndFilm.Room = room;
        roomAndFilm.FilmPath = filmPath;
        return roomAndFilm;
    }
}