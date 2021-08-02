using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Profile.Models.PersonalData
{
    public class ProfileDataViewModel
    {
        public User User { get; set; }
        public Client Client { get; set; }
        public Employee Employee { get; set; }
        public Administrator Administrator { get; set; }
        public Role Role { get; set; }
    }
}
