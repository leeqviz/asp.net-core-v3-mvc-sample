using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Employees;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class EmployeesController : Controller
    {
        private readonly DatabaseContext _context;

        public EmployeesController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> EmployeesList(string search)
        {
            ViewBag.SearchData = search;

            var employees = _context.Employees
                .Include(e => e.User)
                .Join(_context.Storages.Include(s => s.Company), e => e.StorageId, s => s.Id, (e, s) => new EmployeeRecordViewModel
                {
                    UserName = e.User.Name,
                    Email = e.User.Email,
                    CompanyName = s.Company.Name,
                    Id = e.Id,
                    UserId = e.UserId,
                    UniqueKey = e.UniqueKey
                });

            if (!string.IsNullOrEmpty(search))
                employees = employees
                    .Where(e => e.UserName.Contains(search) || e.Email.Contains(search));

            return View(await employees.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddingEmployee()
        {
            var storages = await _context.Storages
                .Include(s => s.Company)
                .ToListAsync();

            var sList = new List<object>();
            foreach (var storage in storages)
            {
                sList.Add(new
                {
                    Id = storage.Id,
                    Name = "Storage: " + storage.Name + " | Company: " + storage.Company.Name
                });
            }
            ViewBag.Storages = new SelectList(sList, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingEmployee(NewEmployeeRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User sameUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Name.Equals(model.Name));
                if (sameUser == null)
                {
                    Role role = await _context.Roles
                        .FirstOrDefaultAsync(r => r.Name.Equals("employee"));
                    if (role == null) return NotFound();

                    User user = new User
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        ImagePath = "~/src/img/profiles/default.png",
                        Role = role,
                        RegistrationDate = DateTime.Now
                    };
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    string key = "eml-0";
                    Employee lastEmployee = await _context.Employees
                           .OrderByDescending(c => c.Id)
                           .FirstOrDefaultAsync();
                    if (lastEmployee != null) key = lastEmployee.UniqueKey;
                    int number = Convert.ToInt32(key[4..]);

                    Employee employee = new Employee
                    {
                        User = user,
                        UniqueKey = "eml-" + ++number,
                        StorageId = model.StorageId
                    };
                    await _context.Employees.AddAsync(employee);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("EmployeesList", "Employees");
                }
                else
                    ModelState.AddModelError(string.Empty, "A user with the same name already exists!");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RemovingEmployee(int? id)
        {
            if (id == null) return NotFound();

            Employee employee = await _context.Employees
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingEmployee(Employee employee)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == employee.UserId);
            if (user == null) return NotFound();

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("EmployeesList", "Employees");
        }
    }
}
