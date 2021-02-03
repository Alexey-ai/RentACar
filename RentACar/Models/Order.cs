using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CarID { get; set; }
        public int DriverID { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }
        public int OdometerStart { get; set; }
        public double FuelStart { get; set; }
        [DataType(DataType.Date)]
        public DateTime? OrderReturnDate { get; set; }
        public int? OdometerFinish { get; set; }
        public double? FuelFinish { get; set; }
        public bool Closed { get; set; }
        public int? MaxMileage { get; set; }
        public int? NumberofDays { get; set; }
        public int? ExtraMileage { get; set; }
        public double ExtraFuel
        {
            get
            {
                if (FuelFinish < FuelStart) { return FuelStart-(double)FuelFinish; }
                return 0;
            }
        }
        public int? Price { get; set; }
        public Car Car { get; set; }
        public Driver Driver { get; set; }

    }
}
