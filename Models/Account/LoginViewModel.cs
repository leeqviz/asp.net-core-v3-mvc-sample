using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Пустое поле!")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "Длина строки должна быть 4-32 символа!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Пустое поле!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
