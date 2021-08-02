using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Profile.Models.PersonalHistory;
using WebApp.Models;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Authorize(Roles = "client")]
    public class PersonalHistoryController : Controller
    {
        private readonly DatabaseContext _context;

        public PersonalHistoryController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OperationsHistory(string search)
        {
            ViewBag.SearchData = search;

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            var operations = _context.Operations
                .Where(o => o.ClientId == client.Id)
                .Join(_context.Copies, o => o.CopyId, c => c.Id, (o, c) => new
                {
                    c.Id,
                    c.PublicationId,
                    o.Type,
                    o.Status,
                    o.CreationDate
                })
                .Join(_context.Publications, o => o.PublicationId, p => p.Id, (o, p) => new PersonalOperationViewModel
                {
                    BookName = p.Name,
                    ImagePath = p.ImagePath,
                    CopyId = o.Id,
                    Type = o.Type,
                    Status = o.Status,
                    CreationDate = o.CreationDate,
                    PublicationType = p.Type
                });

            if (!string.IsNullOrEmpty(search))
                operations = operations
                    .Where(o => o.BookName.Contains(search));

            /*
            int pageSize = 4;
            var count = await operations.CountAsync();
            var items = await operations
                .Skip((page - 1) * pageSize)
                .Distinct()
                .Take(pageSize)
                .ToListAsync();
            */
            PersonalHistoryViewModel model = new PersonalHistoryViewModel
            {
                Operations = operations.ToList(),
                //PageViewModel = new PageViewModel(count, page, pageSize),
                Search = search
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ReturnBook(int? id)
        {
            if (id == null) return NotFound();

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            Operation operation = await _context.Operations
                .FirstOrDefaultAsync(o => o.ClientId == client.Id && o.CopyId == id && o.Status.Equals("accepted") && o.Type.Equals("receive"));
            if (operation == null) return NotFound();

            operation.Status = "processing";
            operation.Type = "return";
            operation.CreationDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("OperationsHistory", "PersonalHistory");
        }
    }
}
