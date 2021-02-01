using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;
using System.ComponentModel.DataAnnotations;

namespace RentACar.Controllers
{
    public class OrdersController : Controller
    {
        private readonly RentContext _context;

        public OrdersController(RentContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var rentContext = _context.Orders.Include(o => o.Car).Include(o => o.Driver);
            return View(await rentContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Driver)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CarID"] = new SelectList(_context.Cars, "ID", "FullName");
            ViewData["DriverID"] = new SelectList(_context.Drivers, "ID", "FullName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,CarID,DriverID,OrderDate,OdometerStart,FuelStart")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                var car = await _context.Cars.FindAsync(order.CarID);
                car.InUse = true;
                _context.Update(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarID"] = new SelectList(_context.Cars, "ID", "ID", order.CarID);
            ViewData["DriverID"] = new SelectList(_context.Drivers, "ID", "FName", order.DriverID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["CarID"] = new SelectList(_context.Cars, "ID", "ID", order.CarID);
            ViewData["DriverID"] = new SelectList(_context.Drivers, "ID", "FName", order.DriverID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderID,CarID,DriverID,OrderDate,OdometerStart,FuelStart,OrderReturnDate,OdometerFinish,FuelFinish,NumberofDays,ExtraMileage,Price")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            ViewData["CarID"] = new SelectList(_context.Cars, "ID", "ID", order.CarID);
            ViewData["DriverID"] = new SelectList(_context.Drivers, "ID", "FName", order.DriverID);
            return View(order);
        }
        public async Task<IActionResult> CloseOrder(int? id)
        {
            if (id == null)return NotFound();

            ViewData["CarID"] = new SelectList(_context.Cars, "ID", "FullName");
            ViewData["DriverID"] = new SelectList(_context.Drivers, "ID", "FullName");

            var order = await _context.Orders.Include(x => x.Car).Include(x => x.Driver).FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null) return NotFound();
            return View(order);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CloseOrder([Bind("OrderID,CarID,DriverID,OrderDate,OdometerStart,FuelStart,OrderReturnDate,OdometerFinish,FuelFinish")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Update(order);
                await _context.SaveChangesAsync();
                order.Closed = true;
                order.Price = 0;
                order.ExtraMileage = 0;
                order.MaxMileage = 0;
                var car = await _context.Cars.FindAsync(order.CarID);
                var mileage = order.OdometerFinish - order.OdometerStart;
                var days =(DateTime)order.OrderReturnDate-order.OrderDate;

                order.MaxMileage = days.Days * car.MaxMileageatDay;
                if (mileage > order.MaxMileage) { order.ExtraMileage = mileage - order.MaxMileage; order.Price += car.OverPrice*(order.ExtraMileage/100+1);  }
                if (days.Days > 0) order.NumberofDays = days.Days;                
                order.Price += car.Price * order.NumberofDays;
                if(order.FuelFinish<order.FuelStart)
                {
                    var extrafuel = order.FuelStart - order.FuelFinish;
                    order.Price += Convert.ToInt32(extrafuel * 50);
                }
                _context.Update(order);
                car.InUse = false;               
                var driver = await _context.Drivers.FindAsync(order.DriverID);
                if (mileage != null && mileage > 0) { driver.Mileage += (int)mileage; car.Odometer += (int)mileage; }
                _context.Update(driver);
                _context.Update(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Driver)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            var car = await _context.Cars.FindAsync(order.CarID);
            car.InUse = false;
            _context.Update(car);
            _context.Orders.Remove(order);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderID == id);
        }
    }
}
