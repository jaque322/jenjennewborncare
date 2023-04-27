using jenjennewborncare.Data;
using jenjennewborncare.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace jenjennewborncare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly jenjennewborncareContext _context;

        public HomeController(ILogger<HomeController> logger, jenjennewborncareContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var videos=_context.Videos.ToList();
            return View(videos);
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