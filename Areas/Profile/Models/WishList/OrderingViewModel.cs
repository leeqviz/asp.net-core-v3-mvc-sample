using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Profile.Models.WishList
{
    public class OrderingViewModel
    {
        [Required]
        public string FIO { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int? ClientId { get; set; }
    }
}
