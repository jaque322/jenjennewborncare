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
    public class NanniesController : Controller
    {
        private readonly jenjennewborncareContext _context;

        public NanniesController(jenjennewborncareContext context)
        {
            _context = context;
        }

        // GET: Nannies
        public async Task<IActionResult> Index()
        {
            var jenjennewborncareContext = _context.Nannies.Include(n => n.Image);
            return View(await jenjennewborncareContext.ToListAsync());
        }

        // GET: Nannies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Nannies == null)
            {
                return NotFound();
            }

            var nannie = await _context.Nannies
                .Include(n => n.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nannie == null)
            {
                return NotFound();
            }

            return View(nannie);
        }

        // GET: Nannies/Create
        public IActionResult Create()
        {
            var filteredImages = _context.Images.Where(x => x.Type == "Team");
            ViewBag.ImageIdSelectList = new SelectList(filteredImages, "Id", "FileName");

            return View();
        }

        // POST: Nannies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageId")] Nannie nannie)
        {
           
                _context.Add(nannie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "FileName", nannie.ImageId);
            return View(nannie);
        }

        // GET: Nannies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Nannies == null)
            {
                return NotFound();
            }

            var nannie = await _context.Nannies.FindAsync(id);
            if (nannie == null)
            {
                return NotFound();
            }
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "FileName", nannie.ImageId);
            return View(nannie);
        }

        // POST: Nannies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageId")] Nannie nannie)
        {
            if (id != nannie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nannie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NannieExists(nannie.Id))
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
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "FileName", nannie.ImageId);
            return View(nannie);
        }

        // GET: Nannies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Nannies == null)
            {
                return NotFound();
            }

            var nannie = await _context.Nannies
                .Include(n => n.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nannie == null)
            {
                return NotFound();
            }

            return View(nannie);
        }

        // POST: Nannies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Nannies == null)
            {
                return Problem("Entity set 'jenjennewborncareContext.Nannies'  is null.");
            }
            var nannie = await _context.Nannies.FindAsync(id);
            if (nannie != null)
            {
                _context.Nannies.Remove(nannie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NannieExists(int id)
        {
          return (_context.Nannies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
