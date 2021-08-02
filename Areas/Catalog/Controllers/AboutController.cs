using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database;

namespace WebApp.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class AboutController : Controller
    {
        private readonly DatabaseContext _context;

        public AboutController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Information()
        {
            return View();
        }
    }
}
