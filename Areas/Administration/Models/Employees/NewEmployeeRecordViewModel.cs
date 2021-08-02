using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Administration.Models.Employees
{
    public class NewEmployeeRecordViewModel
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

        [Required(ErrorMessage = "Storage must be selected!")]
        public int StorageId { get; set; }
    }
}
