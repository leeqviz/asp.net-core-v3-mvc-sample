using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Literature : Entity
    {
        [Required]
        public string Name { get; set; }
        public int? Year { get; set; }

        public int? AuthorId { get; set; }
        public Author Author { get; set; }

        public List<Book> Books { get; set; }
    }
}
