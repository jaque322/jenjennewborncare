using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using jenjennewborncare.Data;
using jenjennewborncare.Models;
using System.Security.Claims;
using jenjennewborncare.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace jenjennewborncare.Controllers
{
    
    public class SchedulesController : Controller
    {
        private readonly jenjennewborncareContext _context;

        public SchedulesController(jenjennewborncareContext context)
        {
            _context = context;
        }

        // GET: Schedules
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index()
        {
           
            var jenjennewborncareContext = _context.Schedules.Include(s => s.User);
            return View(await jenjennewborncareContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["UserId"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var services = _context.Services.ToList();
            ViewBag.Services = new SelectList(services, "Id", "ServiceName"); // Replace "Name" with the appropriate property for your service display

            var scheduleViewModel = new ScheduleViewModel
            {
                UserId = ViewData["UserId"].ToString()
            };

            return View(scheduleViewModel);
        }


        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime,UserId,ServiceIds")] ScheduleViewModel scheduleViewModel)
        {
          
                Schedule schedule = new Schedule
                {
                    StartTime = scheduleViewModel.StartTime,
                    EndTime = scheduleViewModel.EndTime,
                    UserId = scheduleViewModel.UserId,
                    ScheduleServices = new List<ScheduleService>()
                };

                foreach (int serviceId in scheduleViewModel.ServiceIds)
                {
                    var scheduleService = new ScheduleService
                    {
                        ScheduleId = schedule.Id,
                        ServiceId = serviceId
                    };
                    schedule.ScheduleServices.Add(scheduleService);
                }

                _context.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            

            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", scheduleViewModel.UserId);
            return View(scheduleViewModel);
        }


        // GET: Schedules/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", schedule.UserId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,UserId")] Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(schedule.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", schedule.UserId);
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Schedules == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Schedules == null)
            {
                return Problem("Entity set 'jenjennewborncareContext.Schedules'  is null.");
            }
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
          return (_context.Schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
