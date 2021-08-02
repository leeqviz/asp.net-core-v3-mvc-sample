using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Role : Entity
    {
        [Required]
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
