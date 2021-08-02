using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Catalog.Models.Content
{
    public class LiteratureViewModel
    {
        public int? AuthorId { get; set; }
        public string AuthorName { get; set; }
        public IEnumerable<Literature> Literatures { get; set; }
    }
}
