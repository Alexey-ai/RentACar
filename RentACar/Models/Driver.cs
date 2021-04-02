using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class Driver
    {
        public int ID { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ""'\s-]*$", ErrorMessage = "Please enter correct name")]
        public string FName { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ""'\s-]*$", ErrorMessage = "Please enter correct name")]
        public string LName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Passport { get; set; }
        public int Mileage { get; set; }
        public string DriveLicense { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthdayDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime AddDate { get; set; }

        public ICollection<Order> Order { get; set; }

        public string FullName
        {
            get
            {
                return ID + "--" + FName + "--" + LName;
            }
        }
        public int Age
        {
            get
            {
                return (DateTime.Now - BirthdayDate).Days / 365;
            }
        }


    }
}
