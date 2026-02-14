using Microsoft.AspNetCore.Mvc;
using Mission06_Christiansen.Data;
using Mission06_Christiansen.Models;
using System.Linq;
using Mission06_Christiansen.Views.Home;

namespace Mission06_Christiansen.Controllers;

public class HomeController : Controller
{
    // Database context used to interact with the Movies table
    private readonly MovieCollectionContext _context;

    // Constructor receives the DB context through dependency injection
    public HomeController(MovieCollectionContext context)
    {
        _context = context;
    }

    /*
        INDEX PAGE
        Displays all movies.
        Also supports:
        - Search (title, category, director)
        - Rating filter
        - Edited filter
    */
    public IActionResult Index(string? search, string? rating, string? edited)
    {
        // Start with all movies as a queryable collection
        var query = _context.Movies.AsQueryable();

        // Search filter (case-insensitive)
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(m =>
                (m.Title != null && m.Title.ToLower().Contains(s)) ||
                (m.Category != null && m.Category.ToLower().Contains(s)) ||
                (m.Director != null && m.Director.ToLower().Contains(s))
            );
        }

        // Rating filter (G, PG, PG-13, R)
        if (!string.IsNullOrWhiteSpace(rating))
        {
            query = query.Where(m => m.Rating == rating);
        }

        // Edited filter (true/false)
        if (!string.IsNullOrWhiteSpace(edited))
        {
            if (edited == "true")
                query = query.Where(m => m.Edited == true);
            else if (edited == "false")
                query = query.Where(m => m.Edited == false);
        }

        // Sort movies alphabetically by title
        var movies = query
            .OrderBy(m => m.Title)
            .ToList();

        // Store current filter values so the form remembers them
        ViewBag.Search = search ?? "";
        ViewBag.Rating = rating ?? "";
        ViewBag.Edited = edited ?? "";

        return View(movies);
    }

    /*
        GET TO KNOW JOEL PAGE
        Static informational page with links and image.
    */
    public IActionResult GetToKnowJoel()
    {
        return View();
    }

    /*
        ADD MOVIE (GET)
        Displays the movie entry form.
    */
    [HttpGet]
    public IActionResult AddMovie()
    {
        return View();
    }

    /*
        ADD MOVIE (POST)
        Validates and saves the movie to the database.
        Then shows a confirmation page.
    */
    [HttpPost]
    public IActionResult AddMovie(Movie movie)
    {
        if (ModelState.IsValid)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();

            // Fixed typo: "Conformation" → "Confirmation"
            return View("Conformation", movie);
        }

        // If validation fails, reload the form with the entered data
        return View(movie);
    }

    /*
        PRIVACY PAGE
        Static page (Team Rocket message).
    */
    public IActionResult Privacy()
    {
        return View();
    }

    /*
        ROCKET PAGE
        Optional fun page for the Team Rocket motto.
    */
    public IActionResult Rocket()
    {
        return View();
    }
}
