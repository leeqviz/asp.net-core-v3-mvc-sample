using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Administration.Models.Copies
{
    public class CopyRecordViewModel
    {
        public int PublicationId { get; set; }
        public int CopyId { get; set; }
        public float? Price { get; set; }
        public int? Count { get; set; }
        public int? Time { get; set; }
        public string Status { get; set; }
        public string PublicationName { get; set; }
        public string CompanyName { get; set; }
        public string CopyUniqueKey { get; set; }
        public string EmployeeUniqueKey { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
