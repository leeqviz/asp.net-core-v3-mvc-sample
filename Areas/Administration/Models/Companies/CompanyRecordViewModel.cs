using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Models.Companies
{
    public class CompanyRecordViewModel
    {
        public Company Company { get; set; }
        public IEnumerable<Storage> Storages { get; set; }
    }
}
