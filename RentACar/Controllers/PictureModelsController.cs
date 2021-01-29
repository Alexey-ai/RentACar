using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;

namespace RentACar.Controllers
{
    public class PictureModelsController : Controller
    {
        private readonly RentContext _context;

        public PictureModelsController(RentContext context)
        {
            _context = context;
        }

        // GET: PictureModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pictures.ToListAsync());
        }

        // GET: PictureModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictureModel = await _context.Pictures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pictureModel == null)
            {
                return NotFound();
            }

            return View(pictureModel);
        }

        // GET: PictureModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PictureModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Path")] PictureModel pictureModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pictureModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pictureModel);
        }

        // GET: PictureModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictureModel = await _context.Pictures.FindAsync(id);
            if (pictureModel == null)
            {
                return NotFound();
            }
            return View(pictureModel);
        }

        // POST: PictureModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Path")] PictureModel pictureModel)
        {
            if (id != pictureModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pictureModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PictureModelExists(pictureModel.Id))
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
            return View(pictureModel);
        }

        // GET: PictureModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pictureModel = await _context.Pictures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pictureModel == null)
            {
                return NotFound();
            }

            return View(pictureModel);
        }

        // POST: PictureModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pictureModel = await _context.Pictures.FindAsync(id);
            _context.Pictures.Remove(pictureModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PictureModelExists(int id)
        {
            return _context.Pictures.Any(e => e.Id == id);
        }
    }
}
