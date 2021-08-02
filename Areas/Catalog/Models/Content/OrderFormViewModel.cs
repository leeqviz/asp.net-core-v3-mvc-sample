using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Catalog.Models.Content
{
    public class OrderFormViewModel
    {
        [Required]
        public string FIO { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int? ClientId { get; set; }
        public int? CopyId { get; set; }
    }
}
