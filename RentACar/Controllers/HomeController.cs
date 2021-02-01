using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RentACar.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using RentACar.Data;

namespace RentACar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RentContext _context;

        public HomeController(ILogger<HomeController> logger, RentContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult InitialBase()
        {
            if (_context.Cars.Any() || _context.Drivers.Any())
            {
                return Redirect("/Home/Index/");
            }
            var Cars = new List<Car>
            {
            new Car{Manufacturer="Ford",Model="Focus",Year=2020,WheelDrive="front",VechicleVolume=1.6,CarType="sedan",Odometer=768,MaxMileageatDay=100,Price=3000,OverPrice=5000,Deposit=10000,InUse=false,InRepair=false,FuelMax=50,CurrentFuel=30,FuelRate=5 },

            new Car{Manufacturer="Reno",Model="Duster",Year=2018,WheelDrive="full",VechicleVolume=2.0,CarType="hachback",Odometer=3768,MaxMileageatDay=100,Price=4000,OverPrice=5000,Deposit=10000,InUse=false,InRepair=false,FuelMax=50,CurrentFuel=30,FuelRate=8 },

            new Car{Manufacturer="Opel",Model="Astra",Year=2017,WheelDrive="front",VechicleVolume=2.0,CarType="sedan",Odometer=6768,MaxMileageatDay=100,Price=3500,OverPrice=5000,Deposit=10000,InUse=false,InRepair=false,FuelMax=50,CurrentFuel=20,FuelRate=7 },

            new Car{Manufacturer="Kia",Model="Optima",Year=2020,WheelDrive="full",VechicleVolume=2.0,CarType="sedan",Odometer=2000,MaxMileageatDay=100,Price=5000,OverPrice=5000,Deposit=10000,InUse=false,InRepair=false,FuelMax=50,CurrentFuel=25,FuelRate=12 },

            };
            Cars.ForEach(s => _context.Cars.Add(s));
            _context.SaveChanges();

            var Drivers = new List<Driver>
            {
            new Driver{FName="Alex",LName="Ivanov", BirthdayDate=DateTime.Parse("1989-05-05"),PhoneNumber="8-900-555-2020",Passport="2809-546237", AddDate=DateTime.Now,Email="1@1.ru",DriveLicense="99 45 213432"},
            new Driver{FName="Olga",LName="Petrova", BirthdayDate=DateTime.Parse("1993-10-05"),PhoneNumber="8-900-555-1000",Passport="2809-343337", AddDate=DateTime.Now,Email="2@1.ru",DriveLicense="99 45 213432"},
            new Driver{FName="Fu",LName="Kim", BirthdayDate=DateTime.Parse("1998-10-10"),PhoneNumber="8-900-555-6767",Passport="2809-234322", AddDate=DateTime.Now,Email="3@1.ru",DriveLicense="99 45 213432"},
            new Driver{FName="Vasil",LName="Johnson", BirthdayDate=DateTime.Parse("2001-12-12"),PhoneNumber="8-900-555-2666",Passport="2809-34333", AddDate=DateTime.Now,Email="4@1.ru",DriveLicense="99 45 213432" }
            };

            Drivers.ForEach(s => _context.Drivers.Add(s));
            _context.SaveChanges();

            return Redirect("/Home/Index/");
        }
    }
}
