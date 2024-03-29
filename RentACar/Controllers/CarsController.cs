﻿using System;
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
    public class CarsController : Controller
    {
        private readonly RentContext _context;
        private bool Admin => HttpContext.User.HasClaim("Admin", bool.TrueString);

        public CarsController(RentContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {

            return View(await _context.Cars.Include(p => p.Pictures).ToListAsync());
        }
        public async Task<IActionResult> AdminPage()
        {
            if (!Admin) return Forbid();
            return View(await _context.Cars.ToListAsync());
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.ID == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            if (!Admin) return Forbid();
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Manufacturer,Model,Year,Vechicle,VechicleVolume,WheelDrive,CarType,Odometer,MaxMileageatDay,Price,OverPrice,Deposit,RegistrationNumber,IdentificationNumber,VinNumber,InsuranceNumber,Color,InUse,InRepair,FuelMax,CurrentFuel,FuelRate,ShortDescription")] Car car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)return NotFound();
            if (!Admin) return Forbid();


            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Manufacturer,Model,Year,Vechicle,WheelDrive,CarType,VechicleVolume,Odometer,MaxMileageatDay,Price,OverPrice,Deposit,RegistrationNumber,IdentificationNumber,VinNumber,InsuranceNumber,Color,InUse,InRepair,FuelMax,CurrentFuel,FuelRate,ShortDescription")] Car car)
        {
            if (id != car.ID)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.ID))
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
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            if (!Admin) return Forbid();

            var car = await _context.Cars
                .FirstOrDefaultAsync(m => m.ID == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            var pictures =  _context.Pictures.Where(x=>x.CarID==id);
            foreach(var pic in pictures)
            {
                _context.Pictures.Remove(pic);
            }
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.ID == id);
        }
    }
}
