using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Models.Authors
{
    public class AuthorDataRecordViewModel
    {
        public Author Author { get; set; }
        public IFormFile PreviewImage { get; set; }
    }
}
