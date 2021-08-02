using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Administration.Models.Employees
{
    public class EmployeeRecordViewModel
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string CompanyName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string UniqueKey { get; set; }
    }
}
