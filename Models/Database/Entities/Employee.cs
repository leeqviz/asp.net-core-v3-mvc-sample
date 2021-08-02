using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Employee : Entity
    {
        [Required]
        public string UniqueKey { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

        public int? StorageId { get; set; }
        public Storage Storage { get; set; }

        public List<Operation> Operations { get; set; }
    }
}
