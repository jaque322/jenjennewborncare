using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jenjennewborncare.Controllers
{
    public class ErrorsController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
