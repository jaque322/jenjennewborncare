 using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using jenjennewborncare.ViewModels;
using jenjennewborncare.Data;
using jenjennewborncare.Models;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles ="admin")]
public class ImagesController : Controller
{
    private readonly jenjennewborncareContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;

    public ImagesController(jenjennewborncareContext context, IWebHostEnvironment hostingEnvironment)
    {
        _context = context;
        _hostingEnvironment = hostingEnvironment;
    }

    // GET: Images
    public async Task<IActionResult> Index()
    {
        return View(await _context.Images.ToListAsync());
    }

    // GET: Images/Create
    public IActionResult Create()
    {
    
      

        return View();
    }

    // POST: Images/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ImageUploadViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            // Add the following lines to create the 'uploads' folder if it doesn't exist
            string uploadsFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            var fileName = Path.GetFileName(viewModel.FileToUpload.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await viewModel.FileToUpload.CopyToAsync(fileStream);
            }

            var image = new Image
            {
                Title = viewModel.Title,
                FileName = fileName,
                Type = viewModel.TypeId

            };

            _context.Add(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    // Other CRUD actions (Edit, Details, Delete) can be added here

    // GET: Images/Edit/1
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var image = await _context.Images.FindAsync(id);
        if (image == null)
        {
            return NotFound();
        }

        var viewModel = new ImageUploadViewModel
        {
            Id = image.Id,
            Title = image.Title,
            TypeId= image.Type

        };

        return View(viewModel);
    }

    // POST: Images/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ImageUploadViewModel viewModel, IFormFile newFileToUpload = null)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        var existingImage = await _context.Images.FindAsync(id);

        // If a new image file is provided, save it and update the file name
        if (newFileToUpload != null)
        {
            var fileName = Path.GetFileName(newFileToUpload.FileName);
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await newFileToUpload.CopyToAsync(fileStream);
            }

            existingImage.FileName = fileName;
        }

        // Update the image title
        existingImage.Title = viewModel.Title;

        // Ensure ModelState is valid after making changes to the existingImage object
        if (existingImage!=null)
        {
            try
            {
                _context.Update(existingImage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(viewModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        return View(viewModel);
    }






    // GET: Images/Delete/1
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var image = await _context.Images
            .FirstOrDefaultAsync(m => m.Id == id);
        if (image == null)
        {
            return NotFound();
        }

        return View(image);
    }

    // POST: Images/Delete/1
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var image = await _context.Images.FindAsync(id);

        // Delete the image file from the 'uploads' folder
        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", image.FileName);
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        _context.Images.Remove(image);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }



    private bool ImageExists(int id)
    {
        return _context.Images.Any(e => e.Id == id);
    }


}
