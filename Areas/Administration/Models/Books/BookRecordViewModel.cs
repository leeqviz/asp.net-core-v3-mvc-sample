using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Models.Books
{
    public class BookRecordViewModel
    {
        public Publication Publication { get; set; }
        public IEnumerable<Literature> Literatures { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}
