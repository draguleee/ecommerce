using ecommerce.Data;
using ecommerce.Data.Roles;
using ecommerce.Data.Services.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class CinemasController : Controller
    {
        // Injections (ICinemasService)
        private readonly ICinemasService _service;

        // Constructor for CinemasController controller
        public CinemasController(ICinemasService service)
        {
            _service = service;
        }

        // GET: cinemas
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        // GET: cinemas/create
        [HttpGet]
        public IActionResult Create() { return View(); }

        // POST: cinemas/create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")] Cinema cinema)
        {
            if (!ModelState.IsValid) return View(cinema);
            await _service.AddAsync(cinema);
            return RedirectToAction(nameof(Index));
        }

        // GET: cinemas/details/id
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var cinemaDetails = await _service.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        // GET: cinemas/edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cinemaDetails = await _service.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails); 
        }

        // POST: cinemas/edit/id
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] Cinema cinema)
        {
            if (!ModelState.IsValid) return View(cinema);
            if (id == cinema.Id) await _service.UpdateAsync(id, cinema);
            return RedirectToAction(nameof(Index));
        }

        // GET: cinemas/delete/id
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var cinemaDetails = await _service.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");
            return View(cinemaDetails);
        }

        // POST: cinemas/delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cinemaDetails = await _service.GetByIdAsync(id);
            if (cinemaDetails == null) return View("NotFound");
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
