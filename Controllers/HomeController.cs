using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Models;
using Wedding_Planner.Context;
using Wedding_Planner.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Wedding_Planner.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
    public IActionResult Index(string? message)
    {
        var user = HttpContext.Session.GetInt32("userId");
        if (user is not null){
            return RedirectToAction("Index", "Weddings");
        }
        ViewBag.Message = message;

        var homePageViewModel = new HomePageViewModel()
        {
            User = new User(),
            LoginUser = new LoginUser(),
        };

        return View("Index", homePageViewModel);
    }

    [HttpPost("register")]
    public IActionResult Register(User newUser)
    {
        if (!ModelState.IsValid)
        {
            // form is invalid
            // show the form again with errors
        if (!ModelState.IsValid)
        {
              var message = string.Join(" | ", ModelState.Values
             .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));
                  Console.WriteLine(message);
        }

            var homePageViewModel = new HomePageViewModel()
            {
                User = new User(),
                LoginUser = new LoginUser(),
            };
            return View("Index", homePageViewModel);
        }

        // form is valid
        // hash the user's password
        var hasher = new PasswordHasher<User>();
        newUser.Password = hasher.HashPassword(newUser, newUser.Password);

        // save the new user to the db
        _context.Users.Add(newUser);
        _context.SaveChanges();

        // login the user by storing their id in session
        HttpContext.Session.SetInt32("userId", newUser.UserId);

        // redirect user to weddings
        // the second argument is the controller name
        return RedirectToAction("Weddings", "Weddings");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if (!ModelState.IsValid)
        {
            // form is invalid
            // send them back to form to show errors
            var homePageViewModel = new HomePageViewModel()
            {
                User = new User(),
                LoginUser = new LoginUser(),
            };
            return RedirectToAction("Index", "Home");
        }

        // form is valid check if email exists
        var user = _context.Users.SingleOrDefault((user) => user.Email == loginUser.Email);

        if (user is null)
        {
            // email does not exist show a message
            return RedirectToAction("Index", new { message = "invalid-credentials" });
        }

        // check their password
        var hasher = new PasswordHasher<User>();

        PasswordVerificationResult result = hasher.VerifyHashedPassword(
            user,
            user.Password,
            loginUser.Password
        );

        if (result == 0)
        {
            // password is incorrect
            return RedirectToAction("Index", new { message = "invalid-credentials" });
        }

        // password is correct
        // login the user by storing their id in session
        HttpContext.Session.SetInt32("userId", user.UserId);

        // redirect user to weddings
        // the second argument is the controller name
        return RedirectToAction("Weddings", "Weddings");
    }

    [HttpGet("logout")]
    public RedirectToActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
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
}
