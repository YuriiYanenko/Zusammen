using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Components.Endpoints;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Zusmmen.Models;
using System.IO;

namespace Zusmmen.Controllers;

public class Video : Controller
{
    // GET
    private ZusammenDbContext _dbContext;

    public Video(ZusammenDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    [HttpPost]
    public IActionResult Room()
    {
        var onlyFilm = new ZusammenDbController(_dbContext).GetFilmById(1);
        var res = onlyFilm.Result.Value;
        return View(res);
    }

    public ActionResult PlayVideo(int id)
    {
        var onlyFilm = new ZusammenDbController(_dbContext).GetFilmById(id);
        string videoPath = onlyFilm.Result.Value.path;
        byte[] bytes = System.IO.File.ReadAllBytes(videoPath);
        return File(bytes, "video/mp4", videoPath);
    }
}