using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Needed for .Include()
using Mission06_Christiansen.Data;
using Mission06_Christiansen.Models;
using System.Linq;

namespace Mission06_Christiansen.Controllers;

public class HomeController : Controller
{
    // Database context used to interact with Movies/Categories tables
    private readonly MovieCollectionContext _context;

    // Constructor receives the DB context through dependency injection
    public HomeController(MovieCollectionContext context)
    {
        _context = context;
    }

    /*
        INDEX PAGE
        - Displays all movies in the provided database
        - Supports:
          • Search (Title, Director, CategoryName)
          • Rating filter
          • Edited filter
        Notes:
        - We use Include(m => m.Category) so Category.CategoryName is available.
    */
    public IActionResult Index(string? search, string? rating, string? edited)
    {
        // Start with all movies, including Category so we can use CategoryName in search/display
        var query = _context.Movies
            .Include(m => m.Category)
            .AsQueryable();

        // Search filter (case-insensitive)
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();

            query = query.Where(m =>
                (m.Title != null && m.Title.ToLower().Contains(s)) ||
                (m.Director != null && m.Director.ToLower().Contains(s)) ||
                (m.Category != null && m.Category.CategoryName.ToLower().Contains(s))
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

            // IMPORTANT: Your view file is Confirmation.cshtml (not Conformation)
            return View("Confirmation", movie);
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
    // EDIT (GET) - shows the form pre-filled
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);

        if (movie == null)
            return NotFound();

        return View(movie);
    }

// EDIT (POST) - saves changes
    [HttpPost]
    public IActionResult Edit(Movie updatedMovie)
    {
        if (ModelState.IsValid)
        {
            _context.Movies.Update(updatedMovie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // If validation fails, reload the edit form
        return View(updatedMovie);
    }

// DELETE (GET) - confirmation screen
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);

        if (movie == null)
            return NotFound();

        return View(movie);
    }

// DELETE (POST) - actually deletes the record
    [HttpPost]
    public IActionResult DeleteConfirmed(int movieId)
    {
        var movie = _context.Movies.FirstOrDefault(m => m.MovieId == movieId);

        if (movie == null)
            return NotFound();

        _context.Movies.Remove(movie);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}