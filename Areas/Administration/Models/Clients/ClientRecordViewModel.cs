using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Administration.Models.Clients
{
    public class ClientRecordViewModel
    {
        public int Id { get; set; }
        public string UniqueKey { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string CategoryName { get; set; }
    }
}
