using Microsoft.AspNetCore.Mvc;
using Wedding_Planner.Context;
using Wedding_Planner.Models;
using Wedding_Planner.Attributes;
using Wedding_Planner.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Wedding_Planner.Controllers
{
    
    public class WeddingsController : Controller
    {
        private readonly ApplicationContext _context;

        public WeddingsController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("weddings")]
        public IActionResult Weddings()
        {
            // Retrieve user ID from session
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = _context.Users.Find(userId);
            ViewBag.UserName = user.FullName; // Store the user's name in the ViewBag
            ViewBag.UserId = userId;
            // Fetch weddings for the logged-in user
            var weddings = _context.Weddings
                .Include(w => w.RSVPs)
                .ThenInclude(a => a.User)
                .ToList();
            var viewModel = new WeddingsPageViewModel()
            {
                User = user,
                Weddings = weddings

            };
            return View("Weddings", viewModel);
        }


        [HttpPost("weddings/rsvp/{weddingId:int}")]
        public IActionResult RSVP(int weddingId)
        {

            // Retrieve the current user ID (assuming you have a way to get the current user)
            int? userId = GetCurrentUserId();

            // Find the wedding
            var wedding = _context.Weddings.Include(w => w.RSVPs).FirstOrDefault(w => w.WeddingId == weddingId);
            if (wedding == null)
            {
                Console.WriteLine("No WEDDING FOUND ");
                return NotFound();
            }

            // Check if the user has already RSVPed
            if (wedding.RSVPs.Any(r => r.UserId == userId))
            {
                Console.WriteLine("USER HAS ALREADY RSVP");
                // User has already RSVPed
                return RedirectToAction("Index");
            }

            // Add the RSVP
            var rsvp = new RSVP
            {
                WeddingId = weddingId,
                UserId = (int)userId
            };
            _context.RSVPs.Add(rsvp);


            // Save changes to the database
            _context.SaveChanges();
            Console.WriteLine("SAVING OUR RSVP");
            // Redirect to the wedding list or a confirmation page
            return RedirectToAction("Index");
        }

        [HttpPost("weddings/unrsvp/{weddingId:int}")]
        public IActionResult UnRSVP(int weddingId)
        {

            // Retrieve the current user ID (assuming you have a way to get the current user)
            int? userId = GetCurrentUserId();

            // Find the wedding
            var wedding = _context.Weddings.Include(w => w.RSVPs).FirstOrDefault(w => w.WeddingId == weddingId);
            if (wedding == null)
            {
                Console.WriteLine("No WEDDING FOUND ");
                return NotFound();
            }

            // find the RSVP
            var rsvp = _context.RSVPs.FirstOrDefault ((r) => r.UserId == userId && r.WeddingId == weddingId);
            _context.RSVPs.Remove(rsvp);


            // Save changes to the database
            _context.SaveChanges();
            Console.WriteLine("Deleting OUR RSVP");
            // Redirect to the wedding list or a confirmation page
            return RedirectToAction("Weddings");
        }

        private int? GetCurrentUserId()
        {
            // Implement your logic to retrieve the current user ID
            var userId = HttpContext.Session.GetInt32("userId");
            if (userId is not null)
            {

                return userId; // Placeholder
            }
            throw new Exception("USERID NOT FOUND");
        }


        [HttpPost("weddings/sort")]
        public RedirectToActionResult SortWeddings(string property)
        {
            return RedirectToAction("Weddings", new { property });
        }

        public List<Wedding> GetSortedWeddings(string property)
        {
            switch (property)
            {
                case "Title":
                    return _context.Weddings
                        .Include(w => w.Creator)
                        .OrderBy(w => w.Title)
                        .ToList();
                case "Date":
                    return _context.Weddings
                        .Include(w => w.Creator)
                        .OrderBy(w => w.Date)
                        .ToList();
                default:
                    return _context.Weddings
                        .Include(w => w.Creator)
                        .OrderBy(w => w.CreatedAt)
                        .ToList();
            }
        }

        [SessionCheck]
        [HttpGet("weddings/new")]
        public ViewResult NewWedding()
        {
            var wedding = new Wedding()
            {
                UserId = (int)HttpContext.Session.GetInt32("userId"),
            };

            return View("NewWedding", wedding);
        }

        [SessionCheck]
        [HttpPost("Wedding/CreateWedding")]
        public IActionResult CreateWedding(Wedding newWedding)
        {
            if (!ModelState.IsValid)
            {
                var wedding = new Wedding()
                {
                    UserId = (int)HttpContext.Session.GetInt32("userId"),
                };
                if (!ModelState.IsValid)
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    Console.WriteLine(message);
                }

                return View("NewWedding", wedding);
            }

            _context.Weddings.Add(newWedding);
            _context.SaveChanges();
            return RedirectToAction("Weddings");
        }

        [SessionCheck]
        [HttpGet("weddings/{weddingId:int}")]
        public IActionResult WeddingDetails(int weddingId)
        {
            var wedding = _context.Weddings
                .Include(w => w.RSVPs)
                .FirstOrDefault(w => w.WeddingId == weddingId);
            if (wedding is null)
            {
                return NotFound();
            }

            var viewModel = new WeddingDetailsViewModel()
            {
                UserId = (int)HttpContext.Session.GetInt32("userId"),
                Wedding = wedding,
                RSVP = new RSVP()
                {
                    UserId = (int)HttpContext.Session.GetInt32("userId"),
                    WeddingId = weddingId,
                }
            };

            return View("WeddingDetails", viewModel);
        }

        [SessionCheck]
        [HttpPost("weddings/delete/{weddingId:int}")]
        public IActionResult DeleteWedding(int weddingId)
        {
            var wedding = _context.Weddings.Find(weddingId);
            if (wedding is null)
            {
                return NotFound();
            }

            _context.Weddings.Remove(wedding);
            _context.SaveChanges();
            return RedirectToAction("Weddings");
        }
    }
}
