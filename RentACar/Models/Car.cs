using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class Car
    {
        public int ID { get; set; }

        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Vechicle { get; set; }       
        public double VechicleVolume { get; set; }
        public string WheelDrive { get; set; }
        public string CarType { get; set; }
        public int Odometer { get; set; }
        public int MaxMileageatDay { get; set; }
        public int Price { get; set; }
        public int OverPrice { get; set; }
        public int Deposit { get; set; }
        public string RegistrationNumber { get; set; }
        public string IdentificationNumber { get; set; }
        public string VinNumber { get; set; }
        public string InsuranceNumber { get; set; }
        public string Color { get; set; }
        public bool InUse { get; set; }
        public bool InRepair { get; set; }
        public double FuelMax { get; set; }
        public double CurrentFuel { get; set; }
        public double FuelRate { get; set; }

        public List<PictureModel> Pictures { get; set; } = new List<PictureModel>();

        public ICollection<Order> Order { get; set; }
        public string FullName
        {
            get
            {
                return ID + "--" + Manufacturer + "--" + Model+"--"+Year;
            }
        }



    }
}
