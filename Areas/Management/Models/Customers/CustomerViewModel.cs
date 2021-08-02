using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Management.Models.ClientsCategories
{
    public class CustomerViewModel
    {
        public string UserName { get; set; }
        public string FIO { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string CategoryName { get; set; }
        public string ImagePath { get; set; }
    }
}
