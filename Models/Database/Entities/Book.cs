using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Book : Entity
    {
        public int? PublicationId { get; set; }
        public Publication Publication { get; set; }

        public int? LiteratureId { get; set; }
        public Literature Literature { get; set; }
    }
}
