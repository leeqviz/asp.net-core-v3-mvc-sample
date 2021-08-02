using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Administration.Models.Books
{
    public class NewBookRecordViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public int PublisherId { get; set; }
        [Required]
        public List<int> Genres { get; set; }
        [Required]
        public List<int> Literatures { get; set; }
        [Required]
        public IFormFile PreviewImage { get; set; }
    }
}
