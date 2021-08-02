using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Catalog.Models.Service
{
    public class CompanyDetailViewModel
    {
        public Company Company { get; set; }
        public IEnumerable<Storage> Storages { get; set; }
    }
}
