using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class DatabaseViewerController : Controller
    {
        private readonly DatabaseContext _context;

        public DatabaseViewerController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult AdministrationInfo()
        {
            return View();
        }
    }
}
