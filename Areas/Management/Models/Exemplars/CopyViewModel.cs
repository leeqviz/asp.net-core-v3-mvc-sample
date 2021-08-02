using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Management.Models.CopiesEditor
{
    public class CopyViewModel
    {
        public int? CopyId { get; set; }
        public int? PublicationId { get; set; }
        public int? Count { get; set; }
        public int? Time { get; set; }
        public float? Price { get; set; }
        public string PublicationName { get; set; }
        public string PublisherName { get; set; }
        public string CopyUniqueKey { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
