using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zusammen.Models;

namespace Zusammen.Controllers;

public class AutorizationController : Controller
{
    private readonly ZusammenDbContext _context;

    public AutorizationController(UserManager<IdentityUser> userManager, ZusammenDbContext context)
    {
        _context = context;
    }


    public async Task<ActionResult> Register(RegisterModel model)
    {
        if (ModelState.IsValid)
        {
            ZusammenDbController dbController = new ZusammenDbController(_context);
            var userToAdd = new users()
            {
                name = model.name,
                email = model.email,
                password = model.password,
                profile_description = "",
                rooms = new List<int>(),
                profile_image_path = "../img/users/std.png"
            };
            dbController.AddUser(userToAdd);
        }

        //return RedirectToAction("Index", "Home");
        
        return RedirectToAction("Login_Sign", "Home");
    }

    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var userDetails =
                await _context.users.SingleOrDefaultAsync(m => m.email == model.email && m.password == model.password);
            HttpContext.Session.SetString("id", userDetails.name);
        }
            return RedirectToAction("Index", "Home");
    }
}