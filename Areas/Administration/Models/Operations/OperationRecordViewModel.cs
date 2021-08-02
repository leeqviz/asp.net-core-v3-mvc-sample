using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Administration.Models.Operations
{
    public class OperationRecordViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string EmployeeUniqueKey { get; set; }
        public string CopyUniqueKey { get; set; }
        public string ClientUniqueKey { get; set; }
    }
}
