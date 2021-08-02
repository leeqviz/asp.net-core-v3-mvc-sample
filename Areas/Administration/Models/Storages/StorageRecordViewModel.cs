using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Models.Storages
{
    public class StorageRecordViewModel
    {
        public Storage Storage { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
    }
}
