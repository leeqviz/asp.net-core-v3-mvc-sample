using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Catalog.Models.Content
{
    public class ContentViewModel
    {
        public IEnumerable<BookViewModel> Books { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public IEnumerable<int> GenresIds { get; set; }
        public string Search { get; set; }
        public string Sort { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
    }
}
