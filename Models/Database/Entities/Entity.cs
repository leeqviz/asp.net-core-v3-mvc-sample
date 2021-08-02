using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public abstract class Entity
    {
        [Required]
        public int Id { get; set; } // primary key
    }
}
