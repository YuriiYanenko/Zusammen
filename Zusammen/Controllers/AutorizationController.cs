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
        ZusammenDbController dbController = new ZusammenDbController(_context);
        LoginModel? login = new LoginModel(); 
        if (ModelState.IsValid)
        {
            var isUserExist = await _context.users.FirstOrDefaultAsync(u => u.nickname == model.name);
            
            if (isUserExist == null)
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
                await Login(login);
            }
            else
                return View("~/Views/Home/Login_Sign.cshtml", tempModel);
        }
        else
        {
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
                 _context.users.SingleOrDefault(m => 
                    m.nickname == model.name &&
                    m.password == PasswordHasher.HashPassword(model.password, PasswordHasher.salt));
            
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