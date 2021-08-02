using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Catalog.Models.Service
{
    public class ServiceViewModel
    {
        public IEnumerable<Company> Companies { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public string Type { get; set; }
    }
}
