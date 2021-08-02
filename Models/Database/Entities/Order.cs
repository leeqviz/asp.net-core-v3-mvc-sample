using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Order : Entity
    {
        public string FIO { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public int? OperationId { get; set; }
        public Operation Operation { get; set; }
    }
}
