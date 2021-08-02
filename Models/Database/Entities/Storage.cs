using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Storage : Entity
    {
        [Required]
        public string Name { get; set; }

        public int? CompanyId { get; set; }
        public Company Company { get; set; }

        public List<Copy> Copies { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
