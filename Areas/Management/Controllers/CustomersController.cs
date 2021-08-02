using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Management.Models.ClientsCategories;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = "employee")]
    public class CustomersController : Controller
    {
        private readonly DatabaseContext _context;

        public CustomersController(DatabaseContext context)
        {
            _context = context;
        }

        private async Task<Employee> GetEmployee()
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.Id);
        }

        public async Task<IActionResult> CustomersList(string search)
        {
            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();
            if (employee.StorageId != null)
            {
                ViewBag.SearchData = search;

                var clients = _context.Clients
                    .Include(c => c.User);
                var customers = clients
                    .Include(c => c.Category);

                var result = _context.Operations
                    .Include(o => o.Employee)
                    .Where(o => o.Employee.StorageId == employee.StorageId)
                    .Join(customers, o => o.CopyId, c => c.Id, (o, c) => new CustomerViewModel
                    {
                        CategoryName = c.Category.Name,
                        FIO = c.FIO,
                        Address = c.Address,
                        PhoneNumber = c.PhoneNumber,
                        UserName = c.User.Name,
                        ImagePath = c.User.ImagePath
                    });

                if (!string.IsNullOrEmpty(search))
                    result = result
                        .Where(r => r.UserName.Contains(search));

                return View(await result.Distinct().ToListAsync());
            }
            else return NotFound();
        }
    }
}
