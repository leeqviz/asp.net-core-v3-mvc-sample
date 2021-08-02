using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Publisher : Entity
    {
        [Required]
        public string Name { get; set; }

        public List<Publication> Publications { get; set; }
    }
}
