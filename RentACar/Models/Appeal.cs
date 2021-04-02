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
        [Required(ErrorMessage ="Поле не может быть пустым")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Имя должно быть более 3х букв")]
        [RegularExpression(@"^[a-zA-Zа-яА-ЯёЁ""'\s-]*$", ErrorMessage = "Пожалуйста введите корректное имя")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым")]
        public string Contact { get; set; }
        [Required(ErrorMessage = "Поле не может быть пустым")]
        [StringLength(300, MinimumLength = 3, ErrorMessage = "Пожалуйста введите корректный текст")]
        public string Text { get; set; }
    }
}
