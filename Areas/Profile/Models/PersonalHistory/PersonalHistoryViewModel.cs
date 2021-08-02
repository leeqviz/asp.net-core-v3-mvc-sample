using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Areas.Profile.Models.PersonalHistory
{
    public class PersonalHistoryViewModel
    {
        public IEnumerable<PersonalOperationViewModel> Operations { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string Search { get; set; }
    }
}
