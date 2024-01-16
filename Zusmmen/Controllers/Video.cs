using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Components.Endpoints;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace Zusmmen.Controllers;

public class Video : Controller
{
    // GET
    public IActionResult Room()
    {   
        return View();
    }
}