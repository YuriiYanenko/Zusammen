using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Zusammen.Models;
using System.Linq;

namespace Zusammen.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ZusammenDbContext _context;

    public HomeController(ILogger<HomeController> logger, ZusammenDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public IActionResult FilmSearch(string searchName)
    {
        var dbController = new ZusammenDbController(_context);
        var findedFilms = dbController.GetFilmByName(searchName);
        return View(findedFilms.Result.Value);
    }

    public IActionResult Login_Sign()
    {
        return View();
    }

    public IActionResult UserProfil()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult FilmView(int filmId)
    {
        var dbController = new ZusammenDbController(_context);
        var filmData = dbController.GetFilmById(filmId);
        return View(filmData.Result.Value);
    }

    [HttpPost]
    public IActionResult FilterFilms(string[] genreArray, string? yearArray)
    {
        var dbController = new ZusammenDbController(_context);
        string[] years;
        if (yearArray != null)
        {
            years = yearArray.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            years = new[] {"1900", "2024" };
        }
        int[] yearsToInt = new int[] { int.Parse(years[0]), int.Parse(years[1]) };
        var filteredByGenres = dbController.GetFilmByGenre(genreArray).Result.Value;
        var filteredByYears = dbController.GetFilmsByYear(yearsToInt).Result.Value;
        return View("FilmSearch", filteredByGenres.Intersect(filteredByYears).ToList());
    }
}
