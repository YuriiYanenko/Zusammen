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
        ZusammenDbController dbController = new ZusammenDbController(_context);
        Console.WriteLine($"Nick: {model.name}");
        LoginModel? login = new LoginModel(); 
        if (ModelState.IsValid)
        {
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
        }
        else
        {
            return RedirectToAction("Login_Sign", "Home");
        }

        Login(login);
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Login(LoginModel model)
    {
        
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
            
            Console.WriteLine($"User: {userDetails.nickname}, id: {HttpContext.Session.GetString("userName")}");
        }
        else
        {
            return RedirectToAction("Login_Sign", "Home");
        }
    
        return RedirectToAction("Index", "Home");
    }
}