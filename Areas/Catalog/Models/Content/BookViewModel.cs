using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Catalog.Models.Content
{
    public class BookViewModel
    {
        public int PublicationId { get; set; }
        public string PublisherName { get; set; }
        public string PublicationName { get; set; }
        public string ImagePath { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string CompanyType { get; set; }
    }
}
