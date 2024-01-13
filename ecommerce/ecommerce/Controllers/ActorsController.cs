using ecommerce.Data;
using ecommerce.Data.Roles;
using ecommerce.Data.Services.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ActorsController : Controller
    {
        // Injections (IActorsService interface)
        private readonly IActorsService _service;       

        // Constructor for ActorsController controller
        public ActorsController(IActorsService service)
        {
            _service = service;
        }

        // GET: actors
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        // GET: actors/create
        [HttpGet]
        public IActionResult Create() { return View(); }

        // POST: actors/create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureUrl,Bio")]Actor actor)
        {
            if (!ModelState.IsValid) return View(actor); 
            await _service.AddAsync(actor);
            return RedirectToAction(nameof(Index));
        }

        // GET: actors/details/id
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        // GET: actors/edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        // POST: actors/edit/id
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureUrl,Bio")] Actor actor)
        {
            if (!ModelState.IsValid) return View(actor);
            if (id == actor.Id) await _service.UpdateAsync(id, actor);
            return RedirectToAction(nameof(Index));
        }

        // GET: actors/delete/id
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        // POST: actors/delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
