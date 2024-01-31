using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Smo;
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
    public async Task<ActionResult<List<films>>> GetFilms()
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
    public async Task<ActionResult<List<films>>> GetFilmByName(string? name)
    {
        // Get all films where name contains specified value and convert it to list.
        var film = await _context.films.Where(e => EF.Functions.Like(e.name.ToLower(), $"%{name.ToLower()}%"))
            .ToListAsync();
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
    [HttpGet]
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
        var userName = HttpContext.Session.GetString("userName");
        Console.WriteLine("The admin is " + userName);
        var film = GetFilmById(filmId).Result.Value;
        var roomList = await _context.rooms.ToListAsync();
        var lastRoom = roomList[roomList.Count - 1];
        var room = new rooms();
        var user = await GetUserData(userName);
        // Автоматичне інкременування id кімнати.
        room.name = $"{HttpContext.Session.GetString("userName")}_{film.name}";
        room.id = lastRoom.id + 1;
        room.admin_id = user.Value.id;
        room.film_id = filmId;
        room.members_id.Add(room.admin_id);
        _context.rooms.Add(room);
        await _context.SaveChangesAsync();
        return View("~/Views/Video/Room.cshtml", CreateCombinedTable(room).Result.Value);
    }

    public async Task<ActionResult<RoomAndFilm>> CreateCombinedTable(rooms room)
    {
        RoomAndFilm roomAndFilm = new RoomAndFilm();
        var filmPath = GetFilmById(room.film_id).Result.Value.video_path;
        roomAndFilm.Room = room;
        roomAndFilm.FilmPath = filmPath;
        return roomAndFilm;
    }

    public async Task<ActionResult<List<films>>> GetFilmByGenre(string[] genres)
    {
        var films = await _context.films.ToListAsync();
        var filteredFilmsList = films.Where(e => ContainsAllGenres(e.genre, genres).Result).ToList();
        return filteredFilmsList;
    }

    public async Task<ActionResult<List<films>>> GetFilmsByYear(int[] years)
    {
        var filteredFilmsList = await _context.films.Where(e => e.year >= years[0] && e.year <= years[1]).ToListAsync();
        return filteredFilmsList;
    }

    public async Task AddUser(users newUser)
    {
        var userExists = _context.users.FindAsync(newUser.nickname);
        if (userExists == null)
        {
            var allUsers = await _context.users.ToListAsync();
            newUser.id = allUsers[allUsers.Count - 1].id + 1;
            newUser.password = PasswordHasher.HashPassword(newUser.password, PasswordHasher.salt);
            Console.WriteLine(PasswordHasher.salt);
            _context.users.Add(newUser);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ActionResult<users>> GetUserData(string userName)
    {
        var user = await _context.users.FindAsync(userName);
        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    public async Task UpdateUser(RedactUserModel data, string userName)
    {
        var user = await GetUserData(userName);
        user.Value.nickname = data.name == null ? user.Value.nickname : data.name;
        user.Value.profile_description = data.about;
        user.Value.profile_image_path = $"../img/users/{data.imageName}";
        await _context.SaveChangesAsync();
    }

    public async Task AddUserToRoom(int roomId, string userName)
    {
        var room = await _context.rooms.FindAsync(roomId);
        var user = await _context.users.FindAsync(userName);
        user.rooms.Add(roomId);
        room.members_id.Add(user.id);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> ContainsAllGenres(string[] filmGenres, string[] filterGenres)
    {
        foreach (var filter in filterGenres)
        {
            if (!filmGenres.Contains(filter))
                return false;
        }

        return true;
    }
}