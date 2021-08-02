using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Administrator : Entity
    {
        [Required]
        public string UniqueKey { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
