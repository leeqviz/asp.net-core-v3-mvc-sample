using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Genre : Entity
    {
        [Required]
        public string Name { get; set; }

        public List<Filter> Filters { get; set; }
    }
}
