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
using Microsoft.Win32;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text;

namespace jenjennewborncare.Controllers
{
    
    public class SchedulesController : Controller
    {
        private readonly jenjennewborncareContext _context;
        private readonly IEmailSender _emailSender;

        public SchedulesController(jenjennewborncareContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
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

            List<string> items = new List<string>();


            foreach (int serviceId in scheduleViewModel.ServiceIds)
                {
                var service = await _context.Services.SingleOrDefaultAsync(s => s.Id == serviceId);
                items.Add(service.ServiceName);

                var scheduleService = new ScheduleService
                    {
                        ScheduleId = schedule.Id,
                        ServiceId = serviceId
                    };
                    schedule.ScheduleServices.Add(scheduleService);
                }

                _context.Add(schedule);
                await _context.SaveChangesAsync();

                ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", scheduleViewModel.UserId);
            //sending email message config

//client Message
            string htmlEmailContent = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Service Request Confirmation</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
        }
        .container {
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f7f7f7;
            border-radius: 5px;
        }
        .header {
            font-size: 24px;
            font-weight: bold;
            color: #333;
        }
        p {
            font-size: 16px;
            color: #333;
            line-height: 1.5;
        }
    </style>
</head>
<body>
    <div class='container'>
        <h1 class='header'>JenJen Newborn Care Services</h1>
        <p>Dear Client,</p>
        <p>Thank you for requesting our services. We have received your request and will review it shortly. Our team will contact you soon to discuss your needs and provide any additional information you may require.</p>
        <p>We appreciate your trust in JenJen Newborn Care Services, and we are committed to providing you with the highest quality of care and support for you and your newborn.</p>
        <p>If you have any questions or need further assistance, please do not hesitate to reach out to us.</p>
        <p>Best regards,</p>
        <p>The JenJen Newborn Care Services Team</p>
    </div>
</body>
</html>";
            //admin message


            // Replace this list with the actual list of items you want to include in the ul tag

            StringBuilder htmlBuilder = new StringBuilder();

            htmlBuilder.AppendLine("<!DOCTYPE html>");
            htmlBuilder.AppendLine("<html lang='en'>");
            htmlBuilder.AppendLine("<head>");
            htmlBuilder.AppendLine("    <meta charset='UTF-8'>");
            htmlBuilder.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            htmlBuilder.AppendLine("    <title>New Service Request</title>");
            htmlBuilder.AppendLine("    <style>");
            htmlBuilder.AppendLine("        body {");
            htmlBuilder.AppendLine("            font-family: Arial, sans-serif;");
            htmlBuilder.AppendLine("            margin: 0;");
            htmlBuilder.AppendLine("            padding: 0;");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("        .container {");
            htmlBuilder.AppendLine("            max-width: 600px;");
            htmlBuilder.AppendLine("            margin: 0 auto;");
            htmlBuilder.AppendLine("            padding: 20px;");
            htmlBuilder.AppendLine("            background-color: #f7f7f7;");
            htmlBuilder.AppendLine("            border-radius: 5px;");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("        .header {");
            htmlBuilder.AppendLine("            font-size: 24px;");
            htmlBuilder.AppendLine("            font-weight: bold;");
            htmlBuilder.AppendLine("            color: #333;");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("        p {");
            htmlBuilder.AppendLine("            font-size: 16px;");
            htmlBuilder.AppendLine("            color: #333;");
            htmlBuilder.AppendLine("            line-height: 1.5;");
            htmlBuilder.AppendLine("        }");
            htmlBuilder.AppendLine("    </style>");
            htmlBuilder.AppendLine("</head>");
            htmlBuilder.AppendLine("<body>");
            htmlBuilder.AppendLine("    <div class='container'>");
            htmlBuilder.AppendLine("<h1 class='header'>JenJen Newborn Care Services - New Service Request</h1>");
            htmlBuilder.AppendLine("        <p>Dear Admin,</p>");
            htmlBuilder.AppendLine("        <p>A new service request has been submitted by a client. Please review the request and take appropriate action, such as contacting the client to discuss their needs and providing additional information as required.</p>");
            htmlBuilder.AppendLine($"<p><b>List of Services requested by: {User.FindFirstValue(ClaimTypes.Email)}</b></p>");


            htmlBuilder.AppendLine("<ul>");

            foreach (string item in items)
            {
                htmlBuilder.AppendLine($"<li>{item}</li>");
            }

            htmlBuilder.AppendLine("</ul>");
            htmlBuilder.AppendLine("        <p>Thank you for your attention to this matter.</p>");
            htmlBuilder.AppendLine("        <p>Best regards,</p>");
            htmlBuilder.AppendLine("        <p>The JenJen Newborn Care Services Team</p>");
            htmlBuilder.AppendLine("    </div>");
            htmlBuilder.AppendLine("</body>");
            htmlBuilder.AppendLine("</html>");

            string adminHtmlEmailContent = htmlBuilder.ToString();




            await _emailSender.SendEmailAsync(User.FindFirstValue(ClaimTypes.Email), "Welcome to JenJen Newborn Care Services", htmlEmailContent);
            await _emailSender.SendEmailAsync("jenjennewborncare@gmail.com", "Jenjennewborncare ADMIN", adminHtmlEmailContent);



            if ((!User.IsInRole("admin")))
            {
                TempData["alertMessage"] = "Service Requested Sucessfully";
                return RedirectToAction("Create", "Schedules");
            }
            else
            {
                return RedirectToAction(nameof(Index));

            }
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
