using jenjennewborncare.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace jenjennewborncare.Controllers
{
    public class Test : Controller
    {

        private readonly jenjennewborncareContext _context;

        public Test(jenjennewborncareContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var jenjennewborncareContext = _context.Services.Include(s => s.ServiceImage);
            return View(await jenjennewborncareContext.ToListAsync());
        }
    }
}
