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
    public class AppealsController : Controller
    {
        private readonly RentContext _context;

        public AppealsController(RentContext context)
        {
            _context = context;
        }

        // GET: Appeals
        public async Task<IActionResult> ViewAll()
        {
            return View(await _context.Appeales.ToListAsync());
        }

        // GET: Appeals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appeal = await _context.Appeales
                .FirstOrDefaultAsync(m => m.ID == id);
            if (appeal == null)
            {
                return NotFound();
            }

            return View(appeal);
        }

        // GET: Appeals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appeals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Email,Text")] Appeal appeal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appeal);
                await _context.SaveChangesAsync();
                return Redirect("~/Home/AboutUS");
            }
            return View(appeal);
        }

        // GET: Appeals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appeal = await _context.Appeales.FindAsync(id);
            if (appeal == null)
            {
                return NotFound();
            }
            return View(appeal);
        }

        // POST: Appeals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Email,Text")] Appeal appeal)
        {
            if (id != appeal.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appeal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppealExists(appeal.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ViewAll));
            }
            return View(appeal);
        }

        // GET: Appeals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appeal = await _context.Appeales
                .FirstOrDefaultAsync(m => m.ID == id);
            if (appeal == null)
            {
                return NotFound();
            }

            return View(appeal);
        }

        // POST: Appeals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appeal = await _context.Appeales.FindAsync(id);
            _context.Appeales.Remove(appeal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ViewAll));
        }

        private bool AppealExists(int id)
        {
            return _context.Appeales.Any(e => e.ID == id);
        }
    }
}
