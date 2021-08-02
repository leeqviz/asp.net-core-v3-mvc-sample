using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database;

namespace WebApp.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = "employee")]
    public class OperationViewerController : Controller
    {
        private readonly DatabaseContext _context;

        public OperationViewerController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult ManagementInfo()
        {
            return View();
        }
    }
}
