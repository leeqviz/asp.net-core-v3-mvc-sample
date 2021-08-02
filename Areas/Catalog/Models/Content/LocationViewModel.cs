using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Catalog.Models.Content
{
    public class LocationViewModel
    {
        public Copy Copy { get; set; }
        public Company Company { get; set; }
        public Storage Storage { get; set; }
    }
}
