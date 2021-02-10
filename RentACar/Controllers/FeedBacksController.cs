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
    public class FeedBacksController : Controller
    {
        private readonly RentContext _context;

        public FeedBacksController(RentContext context)
        {
            _context = context;
        }

        // GET: FeedBacks
        public async Task<IActionResult> Index()
        {
            return View(await _context.FeedBacks.Include(o => o.Car).ToListAsync());
        }
        public async Task<IActionResult> New()
        {
            return View(await _context.FeedBacks.Include(o => o.Car).Where(x=>x.Processed==false).ToListAsync());
        }

        // GET: FeedBacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedBack = await _context.FeedBacks
                .Include(o => o.Car)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (feedBack == null)
            {
                return NotFound();
            }

            return View(feedBack);
        }

        // GET: FeedBacks/Create
        public IActionResult Create()
        {
            ViewData["CarID"] = new SelectList(_context.Cars,"ID","TradeName");
            return View();
        }

        // POST: FeedBacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CarID,Name,Phone,Text,Processed")] FeedBack feedBack)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedBack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(feedBack);
        }
        //Get feeback close
        public async Task<IActionResult> CloseFeedback(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedback = await _context.FeedBacks
                .FirstOrDefaultAsync(m => m.ID == id);
            if (feedback == null)
            {
                return NotFound();
            }
            feedback.Processed = true;
            _context.FeedBacks.Update(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: FeedBacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedBack = await _context.FeedBacks.FindAsync(id);
            if (feedBack == null)
            {
                return NotFound();
            }
            return View(feedBack);
        }

        // POST: FeedBacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CarID,Name,Phone,Text,Processed")] FeedBack feedBack)
        {
            if (id != feedBack.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedBack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedBackExists(feedBack.ID))
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
            return View(feedBack);
        }

        // GET: FeedBacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var feedBack = await _context.FeedBacks
                .Include(o => o.Car)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (feedBack == null)
            {
                return NotFound();
            }

            return View(feedBack);
        }

        // POST: FeedBacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var feedBack = await _context.FeedBacks.FindAsync(id);
            _context.FeedBacks.Remove(feedBack);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedBackExists(int id)
        {
            return _context.FeedBacks.Any(e => e.ID == id);
        }
    }
}
