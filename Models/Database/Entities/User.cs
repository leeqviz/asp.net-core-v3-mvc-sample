using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class User : Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public DateTime RegistrationDate { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }

        public Client Client { get; set; }
        public Employee Employee { get; set; }
        public Administrator Administrator { get; set; }
    }
}
