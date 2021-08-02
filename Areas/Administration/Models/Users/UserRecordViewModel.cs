using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Administration.Models.Users
{
    public class UserRecordViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string ImagePath { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
