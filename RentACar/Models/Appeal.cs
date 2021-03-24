using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RentACar.Models
{
    public class Appeal
    {
        public int ID { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Please enter correct name")]
        public string Name { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3)]
        [RegularExpression(@"^\S+@\S+\.\S+$", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Please enter correct text")]
        public string Text { get; set; }
    }
}
