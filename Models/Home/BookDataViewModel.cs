using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models.Home
{
    public class BookDataViewModel
    {
        public int BookId { get; set; }
        public int PublicationId { get; set; }
        public string PublisherName { get; set; }
        public string BookName { get; set; }
        public string PreviewImage { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
    }
}
