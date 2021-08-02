using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Profile.Models.PersonalData
{
    public class DataViewModel
    {
        [Required(ErrorMessage = "Empty field!")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "The string length must be between 4 and 32 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Empty field!")]
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Incorrect format!")]
        [StringLength(32, MinimumLength = 4, ErrorMessage = "The string length must be between 4 and 32 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Empty field!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ProfileImage { get; set; }
        public int? Age { get; set; }
        public string FIO { get; set; }
        public string Address { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public IFormFile Avatar { get; set; } // for file load
        public int? CategoryId { get; set; }
    }
}
