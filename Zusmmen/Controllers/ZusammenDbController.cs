using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Zusmmen.Models;

namespace Zusmmen.Controllers;

public class ZusammenDbController : Controller
{
    //Context of Zusammen database (sets of tables objects).
    private readonly ZusammenDbContext _context;

    //Constructor, get db context from localhost:5344/zusammen. 
    public ZusammenDbController(ZusammenDbContext context)
    {
        _context = context;
    }
    
    //Gets List of values from films table.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<films>>> GetFilms()
    {
        return await _context.films.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<films>> GetFilmById(int id)
    {
        var film = await _context.films.FindAsync(id);
        if (null == film)
        {
            return NotFound();
        }

        return film;
    }

    [HttpPost]
    public async Task<IActionResult> AddFilm(films filmToAdd)
    {
        _context.films.Add(filmToAdd);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetFilmById", new { id = filmToAdd.id }, filmToAdd);
    }
}