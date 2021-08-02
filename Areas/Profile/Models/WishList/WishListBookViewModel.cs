using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Profile.Models.WishList
{
    public class WishListBookViewModel
    {
        public string PublicationName { get; set; }
        public string ImagePath { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyName { get; set; }
        public string CompanyPhoneNumber { get; set; }
        public string CompanyType { get; set; }
        public float? Price { get; set; }
        public int? Count { get; set; }
        public int CountInCart { get; set; }
        public int? Time { get; set; }
        public int PublicationId { get; set; }
        public int CopyId { get; set; }
        public int CartId { get; set; }
    }
}
