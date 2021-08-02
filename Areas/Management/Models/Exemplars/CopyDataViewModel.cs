using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Areas.Management.Models.CopiesEditor
{
    public class CopyDataViewModel
    {
        public float? Price { get; set; }
        public int? Time { get; set; }

        [Required(ErrorMessage = "Empty field!")]
        public int? Count { get; set; }
        public int? CopyId { get; set; }
        [Required(ErrorMessage = "Empty field!")]
        public int? PublicationId { get; set; }
    }
}
