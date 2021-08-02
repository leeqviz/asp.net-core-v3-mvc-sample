using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Category : Entity
    {
        [Required]
        public string Name { get; set; }

        public List<Client> Clients { get; set; }
    }
}
