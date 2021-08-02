using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Operation : Entity
    {
        [Required]
        public string Type { get; set; } // purchase / receive / return
        [Required]
        public string Status { get; set; } // processing / accepted / denied
        [Required]
        public DateTime CreationDate { get; set; }

        public int? CopyId { get; set; }
        public Copy Copy { get; set; }

        public int? ClientId { get; set; }
        public Client CLient { get; set; }

        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public Order Order { get; set; }
    }
}
