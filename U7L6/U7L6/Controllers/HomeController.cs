using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using U7L6.Models;

namespace U7L6.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MoviesDbContext _MoviesDbContext;

        public HomeController(ILogger<HomeController> logger, MoviesDbContext moviesDbContext)
        {
            _logger = logger;
            _MoviesDbContext = moviesDbContext;
        }

        public IActionResult Index()
        {
            return View();
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
}