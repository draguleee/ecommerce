using ecommerce.Data;
using Microsoft.AspNetCore.Mvc;

namespace ecommerce.Controllers
{
    public class HomeController : Controller
    {
        // Injections (logger)
        private readonly ILogger<HomeController> _logger;

        // Constructor for HomeController controller
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: home
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
