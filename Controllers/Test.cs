using Microsoft.AspNetCore.Mvc;

namespace jenjennewborncare.Controllers
{
    public class Test : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
