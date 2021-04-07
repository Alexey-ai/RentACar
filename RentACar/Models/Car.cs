using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RentACar.Models
{
    public class Car
    {
        public int ID { get; set; }

        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Vechicle { get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [RegularExpression(@"^[0-9]*[,]?[0-9]+$", ErrorMessage = "Числа вводятся в формате x или x,x")]
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
        public string ShortDescription { get; set; }

        public List<PictureModel> Pictures { get; set; } = new List<PictureModel>();

        public ICollection<Order> Order { get; set; }
        public string FullName
        {
            get
            {
                return ID + "--" + Manufacturer + "--" + Model+"--"+Year;
            }
        }
        public string TradeNameYear
        {
            get
            {
                return Manufacturer + "-" + Model + "-" + Year;
            }
        }
        public string TradeName
        {
            get
            {
                return Manufacturer + "-" + Model;
            }
        }



    }
}
