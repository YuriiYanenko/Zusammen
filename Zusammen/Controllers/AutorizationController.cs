using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;
using Zusammen.Models;

namespace Zusammen.Controllers;

public class AutorizationController : Controller
{
    private readonly ZusammenDbContext _context;

    public AutorizationController(ZusammenDbContext context)
    {
        _context = context;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterModel model)
    {
        var tempModel = new RegAndLogModel() { register = model };
        if (ModelState.IsValid)
        {
            ZusammenDbController dbController = new ZusammenDbController(_context);
            LoginModel? login = new LoginModel(); 
            
            var userToAdd = new users()
            {
                nickname = model.name,
                email = model.email,
                password = model.password,
                profile_description = "",
                rooms = new List<int>(),
                profile_image_path = "../img/users/std.png",
                status = "offline"
            };
            await dbController.AddUser(userToAdd);
            
            login.name = model.name;
            login.password = model.password;
            await Login(login);
        }
        else
        {
            ModelState.AddModelError(String.Empty, "error");
            return View("~/Views/Home/Login_Sign.cshtml", tempModel);
        }
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Login(LoginModel model)
    {
        var tempModel = new RegAndLogModel() { login = model };
        var dbController = new ZusammenDbController(_context);
        if (ModelState.IsValid)
        {   
            var userDetails =
                await _context.users.SingleOrDefaultAsync(m => 
                    m.nickname == model.name &&
                    m.password == PasswordHasher.HashPassword(model.password, PasswordHasher.salt));
            Console.WriteLine($"Hash: {dbController.HashPassword(model.password)}");
            if (userDetails == null)
            {
                ModelState.AddModelError("Password", "Invalid login attempt.");
                return RedirectToAction("Login_Sign", "Home");
            }

            HttpContext.Session.SetString("userName", userDetails.nickname);
            userDetails.status = "online";
            await _context.SaveChangesAsync();
        }
        else
        {
            return View("~/Views/Home/Login_Sign.cshtml", tempModel);
        }
    
        return RedirectToAction("Index", "Home");
    }
}