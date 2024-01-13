using ecommerce.Data;
using ecommerce.Data.Roles;
using ecommerce.Data.Services.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class MoviesController : Controller
    {
        // Injections (IMoviesService interface)
        private readonly IMoviesService _service;

        // Constructor for MoviesController controller
        public MoviesController(IMoviesService service)
        {
            _service = service;
        }

        // GET: movies
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var data = await _service.GetAllAsync(n => n.Cinema);
            return View(data);
        }

        // GET: movies
        [AllowAnonymous]
        public async Task<ActionResult> Filter(string searchString)
        {
            var data = await _service.GetAllAsync(n => n.Cinema);
            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredData = data
                    .Where(n => n.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                    || n.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                return View("Index", filteredData);
            }
            return View("Index", data);
        }

        // GET: movies/create
        [HttpGet]
        public async Task<IActionResult> Create() 
        {
            var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
            return View(); 
        }

        // POST: movies/create
        [HttpPost]
        public async Task<IActionResult> Create(NewMovieVM movie)
        {
            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                return View(movie);
            }
            await _service.AddNewMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        // GET: movies/details/id
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            if (movieDetails == null) return View("NotFound");
            return View(movieDetails);
        }

        // GET: movies/edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movieDetails = await _service.GetMovieByIdAsync(id);
            if (movieDetails == null) return View("NotFound");
            var response = new NewMovieVM()
            {
                Id = movieDetails.Id,
                Name = movieDetails.Name,
                Description = movieDetails.Description,
                Price = movieDetails.Price,
                StartDate = movieDetails.StartDate,
                EndDate = movieDetails.EndDate,
                ImageUrl = movieDetails.ImageUrl,
                MovieCategory = movieDetails.MovieCategory,
                CinemaId = movieDetails.CinemaId,
                ProducerId = movieDetails.ProducerId,
                ActorIds = movieDetails.Actors_Movies.Select(n => n.ActorId).ToList()
            };
            var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
            ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
            ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
            ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
            return View(response);
        }

        // POST: movies/edit/id
        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewMovieVM movie)
        {
            if (id != movie.Id) return View("NotFound");
            if (!ModelState.IsValid)
            {
                var movieDropdownsData = await _service.GetNewMovieDropdownsValues();
                ViewBag.Producers = new SelectList(movieDropdownsData.Producers, "Id", "FullName");
                ViewBag.Cinemas = new SelectList(movieDropdownsData.Cinemas, "Id", "Name");
                ViewBag.Actors = new SelectList(movieDropdownsData.Actors, "Id", "FullName");
                return View(movie);
            }
            await _service.UpdateMovieAsync(movie);
            return RedirectToAction(nameof(Index));
        }
    }
}
