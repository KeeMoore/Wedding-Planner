using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Context;
using Wedding_Planner.Models;
using Wedding_Planner.Attributes;
using Wedding_Planner.ViewModels;
using Microsoft.EntityFrameworkCore;

public class UsersController : Controller
{
    private readonly ApplicationContext _context;

    public UsersController(ApplicationContext context)
    {
        _context = context;
    }

    // Registration and Login actions here

    [HttpGet]
    public IActionResult Register() 
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        if (ModelState.IsValid)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(user);
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(User user)
    {
        var loggedInUser = HttpContext.Session.GetInt32("userId");
        if (loggedInUser != null)
        {
            // HttpContext.Session.SetInt32("UserId", loggedInUser.UserId);
            // HttpContext.Session.SetString("UserName", loggedInUser.Name); // Store the user's name in the session
            return RedirectToAction("Index", "Weddings");
        }
        ModelState.AddModelError("", "Invalid login attempt.");
        return View(user);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}

