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
using Microsoft.AspNetCore.Authorization;

namespace RentACar.Controllers
{ 
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly RentContext _context;
        private bool Admin => HttpContext.User.HasClaim("Admin", bool.TrueString);
        public OrdersController(RentContext context)
        {
            _context = context;
        }

        // GET: All Orders
        public async Task<IActionResult> Index()
        {
            if (!Admin) return Forbid();

            var rentContext = _context.Orders.Include(o => o.Car).Include(o => o.Driver);
            return View(await rentContext.ToListAsync());
        }
        // GET: Orders in work
        public async Task<IActionResult> InWork()
        {
            if (!Admin) return Forbid();

            var rentContext = _context.Orders.Include(o => o.Car).Include(o => o.Driver).Where(x => x.Closed != true);
            return View(await rentContext.ToListAsync());
        }
        // GET: Closed Orders
        public async Task<IActionResult> Closed()
        {
            if (!Admin) return Forbid();

            var rentContext = _context.Orders.Include(o => o.Car).Include(o => o.Driver).Where(x => x.Closed == true);
            return View(await rentContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (!Admin) return Forbid();
            if (id == null)return NotFound();

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
            if (!Admin) return Forbid();
            ViewData["CarID"] = new SelectList(_context.Cars.Where(x=>x.InUse!=true&&x.InRepair!=true), "ID", "FullName");
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
            if (!Admin) return Forbid();
            if (id == null) return NotFound();

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
            if (!Admin)return Forbid();
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
                order.Closed = true;
                order.Price = 0;
                order.ExtraMileage = 0;
                order.MaxMileage = 0;
                var car = await _context.Cars.FindAsync(order.CarID);
                var mileage = order.OdometerFinish - order.OdometerStart;
                var days =(DateTime)order.OrderReturnDate-order.OrderDate;
                
                if (days.Days >= 0) order.NumberofDays = days.Days;
                if (order.NumberofDays == 0) { order.NumberofDays = 1; }

                order.MaxMileage = order.NumberofDays * car.MaxMileageatDay;
                if (mileage > order.MaxMileage) {
                    order.ExtraMileage = mileage - order.MaxMileage;
                    if (order.ExtraMileage % 100 == 0)
                    {
                        order.Price += car.OverPrice * order.ExtraMileage / 100;
                    }
                    else order.Price += car.OverPrice * (order.ExtraMileage / 100+1);
                }
                
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

        public async Task<IActionResult> PayInfo(int? id)
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
        public async Task<IActionResult> GetPay(int? id)
        {
            Pay pay = new Pay();

            var order = await _context.Orders
                .Include(o => o.Car)
                .Include(o => o.Driver)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            pay.Date = order.OrderDate;
            pay.Model = order.Car.FullName;
            pay.Name = order.Driver.FullName;
            pay.Price = (int)order.Price;

            System.Xml.Serialization.XmlSerializer xml = new System.Xml.Serialization.XmlSerializer(typeof(Pay));
            var stream = new System.IO.MemoryStream();
            xml.Serialize(stream, pay);
            stream.Flush();
            var data = stream.GetBuffer();
            stream.Close();
            string name = order.OrderDate.Date.Month.ToString()+" "+order.OrderDate.Date.Day.ToString()+"_"+ order.OrderID;
            return File(data, "application/xhtml+xml", name+".xml");
        }
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!Admin) return Forbid();
            if (id == null) return NotFound();

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
