using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.Metrics;
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
            var isUserExist = await _context.users.FirstOrDefaultAsync(u => u.nickname == model.name) == null
                ? true
                : false;
            
            if (isUserExist)
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
                login.email = model.email;
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
                    m.email == model.email &&
                    m.password == PasswordHasher.HashPassword(model.password));
            
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
    
    // Повертає true якщо існує користувач із заданим ім'ям.
    public bool IsUserExist(string userName)
    {
        return _context.users.FirstOrDefault(v => v.nickname == userName) == null;
    }

    // Повертає true кщо існує користувач із заданою поштою.
    public bool IsEmailExist(string email)
    {
        return _context.users.FirstOrDefault(v => v.email == email) == null;
    }

    // Визначає чи правильно введено пароль для користувача.
    public bool IsPasswordCorrect(string userName, string password)
    {
        var hashedPassword = PasswordHasher.HashPassword(password);
        return _context.users.FirstOrDefault(v => v.nickname == userName && v.password == hashedPassword) == null;
    }
}