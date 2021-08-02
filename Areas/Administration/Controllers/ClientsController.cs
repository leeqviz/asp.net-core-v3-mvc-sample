using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Clients;
using WebApp.Models.Database;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class ClientsController : Controller
    {
        private readonly DatabaseContext _context;

        public ClientsController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ClientsList(string search)
        {
            ViewBag.SearchData = search;

            var clients = _context.Clients
                .Include(c => c.Category)
                .Join(_context.Users, c => c.UserId, u => u.Id, (c, u) => new ClientRecordViewModel
                { 
                    CategoryName = c.Category.Name,
                    UniqueKey = c.UniqueKey,
                    Email = u.Email,
                    Id = c.Id,
                    UserName = u.Name
                });

            if (!string.IsNullOrEmpty(search))
                clients = clients
                    .Where(c => c.UserName.Contains(search) || c.Email.Contains(search));

            return View(await clients.ToListAsync());
        }
    }
}
