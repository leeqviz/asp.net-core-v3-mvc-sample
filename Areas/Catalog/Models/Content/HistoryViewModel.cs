using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Catalog.Models.Content
{
    public class HistoryViewModel
    {
        public string UserName { get; set; }
        public string UserProfileImage { get; set; }
        public string Comment { get; set; }
        public int? Rating { get; set; }
    }
}
