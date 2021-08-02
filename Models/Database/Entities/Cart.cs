using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Database.Entities
{
    public class Cart : Entity
    {
        public int Count { get; set; }

        public int? CopyId { get; set; }
        public Copy Copy { get; set; }

        public int? ClientId { get; set; }
        public Client Client { get; set; }
    }
}
