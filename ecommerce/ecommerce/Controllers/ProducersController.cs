using ecommerce.Data;
using ecommerce.Data.Roles;
using ecommerce.Data.Services.Interfaces;
using ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class ProducersController : Controller
    {
        // Injections (IProducersService interface)
        private readonly IProducersService _service;

        // Constructor for ProducersController controller
        public ProducersController(IProducersService service)
        {
            _service = service;
        }

        // GET: producers
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        // GET: producers/create
        [HttpGet]
        public IActionResult Create() { return View(); }

        // POST: producers/create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,ProfilePictureUrl,Bio")] Producer producer)
        {
            if (!ModelState.IsValid) return View(producer);
            await _service.AddAsync(producer);
            return RedirectToAction(nameof(Index));
        }

        // GET: producers/details/id
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        // GET: producers/edit/id
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        // POST: producers/edit/id
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,ProfilePictureUrl,Bio")] Producer producer)
        {
            if (!ModelState.IsValid) return View(producer);
            if (id == producer.Id) await _service.UpdateAsync(id, producer);
            return RedirectToAction(nameof(Index));
        }

        // GET: producers/delete/id
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            return View(producerDetails);
        }

        // POST: producers/delete
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null) return View("NotFound");
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
