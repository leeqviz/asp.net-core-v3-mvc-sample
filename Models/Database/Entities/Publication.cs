using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Publication : Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public int Year { get; set; }

        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public History History { get; set; }

        public List<Filter> Filters { get; set; }
        public List<Book> Books { get; set; }
        public List<Copy> Copies { get; set; }
    }
}
