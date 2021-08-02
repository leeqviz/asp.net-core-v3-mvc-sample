using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Client : Entity
    {
        [Required]
        public string UniqueKey { get; set; }
        public string FIO { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? Age { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
        
        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public History History { get; set; }

        public List<Cart> Carts { get; set; }
        public List<Operation> Operations { get; set; }
    }
}
