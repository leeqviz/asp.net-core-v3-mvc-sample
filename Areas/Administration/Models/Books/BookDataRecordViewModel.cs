using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Models.Books
{
    public class BookDataRecordViewModel
    {
        [Required]
        public IFormFile PreviewImage { get; set; }
        public Publication Publication { get; set; }
        [Required]
        public List<int> Genres { get; set; }
        [Required]
        public List<int> Literatures { get; set; }
    }
}
