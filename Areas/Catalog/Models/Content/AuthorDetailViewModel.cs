using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Catalog.Models.Content
{
    public class AuthorDetailViewModel
    {
        public Author Author { get; set; }
        public IEnumerable<Literature> Literatures { get; set; }
    }
}
