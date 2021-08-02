using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Catalog.Models.Content
{
    public class BookDetailViewModel
    {
        public Publication Publication { get; set; }
        public History PersonalHistory { get; set; }
        public IEnumerable<HistoryViewModel> Histories { get; set; }
        public IEnumerable<LiteratureViewModel> Literatures { get; set; }
        public IEnumerable<LocationViewModel> Locations { get; set; }
    }
}
