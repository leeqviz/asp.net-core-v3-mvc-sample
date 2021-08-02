using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Profile.Models.PersonalHistory
{
    public class PersonalOperationViewModel
    {
        public int CopyId { get; set; }
        public string BookName { get; set; }
        public string ImagePath { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string PublicationType { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
