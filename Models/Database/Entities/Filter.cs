using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Filter : Entity
    {
        public int? PublicationId { get; set; }
        public Publication Publication { get; set; }

        public int? GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
