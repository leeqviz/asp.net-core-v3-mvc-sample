using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Account
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "Пустое поле!")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "The string length must be between 4 and 32 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пустое поле!")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Неверный формат!")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Длина строки должна быть 4-32 символа!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пустое поле!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Неверное значение!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
