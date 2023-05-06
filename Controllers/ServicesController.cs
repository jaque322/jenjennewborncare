using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jenjennewborncare.Data;
using jenjennewborncare.Models;
using Microsoft.AspNetCore.Authorization;

namespace jenjennewborncare.Controllers
{
    [Authorize(Roles = "admin")]
    public class ServicesController : Controller
    {
        private readonly jenjennewborncareContext _context;

        public ServicesController(jenjennewborncareContext context)
        {
            _context = context;
        }

        // GET: Services
        public async Task<IActionResult> Index()
        {
            var jenjennewborncareContext = _context.BabyCareServices.Include(s => s.ServiceImage);
            return View(await jenjennewborncareContext.ToListAsync());
        }

        // GET: Services/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BabyCareServices == null)
            {
                return NotFound();
            }

            var service = await _context.BabyCareServices
                .Include(s => s.ServiceImage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // GET: Services/Create
        public IActionResult Create()
        {
            var filteredImages = _context.Images.Where(x => x.Type == "Services");
            ViewData["ServiceImageId"] = new SelectList(filteredImages, "Id", "FileName");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ServiceName,ServiceContent,ProviderName,Price,DateCreated,ServiceImageId")] Service service)
        {
           
                _context.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["ServiceImageId"] = new SelectList(_context.Images, "Id", "FileName", service.ServiceImageId);
            return View(service);
        }

        // GET: Services/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BabyCareServices == null)
            {
                return NotFound();
            }

            var service = await _context.BabyCareServices.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            ViewData["ServiceImageId"] = new SelectList(_context.Images, "Id", "FileName", service.ServiceImageId);
            return View(service);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ServiceName,ServiceContent,ProviderName,Price,DateCreated,ServiceImageId")] Service service)
        {
            if (id != service.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(service);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServiceExists(service.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceImageId"] = new SelectList(_context.Images, "Id", "FileName", service.ServiceImageId);
            return View(service);
        }

        // GET: Services/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BabyCareServices == null)
            {
                return NotFound();
            }

            var service = await _context.BabyCareServices
                .Include(s => s.ServiceImage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        // POST: Services/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BabyCareServices == null)
            {
                return Problem("Entity set 'jenjennewborncareContext.BabyCareServices'  is null.");
            }
            var service = await _context.BabyCareServices.FindAsync(id);
            if (service != null)
            {
                _context.BabyCareServices.Remove(service);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ServiceExists(int id)
        {
          return (_context.BabyCareServices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
