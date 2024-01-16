using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Zusmmen.Models;

namespace Zusmmen.Controllers;

public class ZusammenDbController : Controller
{
    private readonly ZusammenDbContext _context;

    public ZusammenDbController(ZusammenDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Films>>> GetFilms()
    {
        return await _context.Films.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Films>> GetFilmById(int id)
    {
        var film = await _context.Films.FindAsync(id);
        if (null == film)
        {
            return NotFound();
        }

        return film;
    }

    [HttpPost]
    public async Task<IActionResult> AddFilm(Films filmToAdd)
    {
        _context.Films.Add(filmToAdd);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetFilmById", new { id = filmToAdd.Id }, filmToAdd);
    }
}