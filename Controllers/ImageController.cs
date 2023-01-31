using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReCAPTCHA.Models;

namespace ReCAPTCHA.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageDbContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageController(ImageDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _webHostEnvironment = hostEnvironment; 

        }

        // GET: Image
        public async Task<IActionResult> Index()
        {
              return _context.Images != null ? 
                          View(await _context.Images.ToListAsync()) :
                          Problem("Entity set 'ImageDbContext.Images'  is null.");
        }

        // GET: Image/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }

        // GET: Image/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Image/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,Title,ImageFile,TypeId,TypeName")] ImageModel imageModel)
        {
            if (ModelState.IsValid)
            {
                // Save Image to wwwroot/image
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                string filename = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);

                string extension = Path.GetExtension(imageModel.ImageFile.FileName);

                imageModel.ImageName = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;

                string path = Path.Combine(wwwRootPath + "/Image/", filename);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }

                // Insert Record

                _context.Add(imageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageModel);
        }

        // GET: Image/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images.FindAsync(id);
            if (imageModel == null)
            {
                return NotFound();
            }
            return View(imageModel);
        }

        // POST: Image/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageId,Title,ImageFile,TypeId,TypeName")] ImageModel imageModel)
        {
            if (id != imageModel.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageModelExists(imageModel.ImageId))
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
            return View(imageModel);
        }

        // GET: Image/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Images == null)
            {
                return NotFound();
            }

            
            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.ImageId == id);

    

            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }

        // POST: Image/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Images == null)
            {
                return Problem("Entity set 'ImageDbContext.Images'  is null.");
            }
            var imageModel = await _context.Images.FindAsync(id);

            // delete image from wwwroot/image
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "image", imageModel.ImageName);

            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            if (imageModel != null)
            {
                _context.Images.Remove(imageModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageModelExists(int id)
        {
          return (_context.Images?.Any(e => e.ImageId == id)).GetValueOrDefault();
        }
    }
}
