using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class FeedBack
    {
        public int ID { get; set; }
        public int CarID { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ""'\s-]*$", ErrorMessage = "Please enter correct name")]
        public string Name { get; set; }
        
        [Required]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Please enter correct phone")]
        
        public string Phone { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Please enter correct text")]
        public string Text { get; set; }
        public bool Processed { get; set; }
        public Car Car { get; set; }
    }
}
