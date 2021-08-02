using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class History : Entity
    {
        public string Comment { get; set; }
        public int? Rating { get; set; } = 0;

        public int? PublicationId { get; set; }
        public Publication Publication { get; set; }

        public int? ClientId { get; set; }
        public Client Client { get; set; }
    }
}
