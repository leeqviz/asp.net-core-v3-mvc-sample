using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Management.Models.OperationsViewer
{
    public class OperationViewModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string CopyUniqueKey { get; set; }
        public string ClientUniqueKey { get; set; }
        public Order Order { get; set; }
    }
}
