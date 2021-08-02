using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Copy : Entity
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public DateTime ModifiedDate { get; set; }
        [Required]
        public string UniqueKey { get; set; }
        public string Status { get; set; } // available / written-off
        public float? Price { get; set; } = 0.0f;
        public int? Count { get; set; } = 0;
        public int? Time { get; set; } = 14;

        public int? PublicationId { get; set; }
        public Publication Publication { get; set; }

        public int? StorageId { get; set; }
        public Storage Storage { get; set; }

        public List<Cart> Carts { get; set; }
        public List<Operation> Operations { get; set; }
    }
}
