using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Administrators;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class AdministratorsController : Controller
    {
        private readonly DatabaseContext _context;

        public AdministratorsController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AdministratorsList(string search)
        {
            ViewBag.SearchData = search;

            var administrators = _context.Administrators
                .Include(a => a.User)
                .Select(a => new AdministratorRecordViewModel
                {
                    Id = a.Id,
                    Email = a.User.Email,
                    UniqueKey = a.UniqueKey,
                    UserName = a.User.Name
                });

            if (!string.IsNullOrEmpty(search))
                administrators = administrators
                    .Where(a => a.UserName.Contains(search) || a.Email.Contains(search));

            return View(await administrators.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddingAdministrator() 
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingAdministrator(NewAdministratorRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User sameUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Name.Equals(model.Name));
                if (sameUser == null)
                {
                    Role role = await _context.Roles
                        .FirstOrDefaultAsync(r => r.Name.Equals("admin"));
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

                    string key = "adm-0";
                    Administrator lastAdmin = await _context.Administrators
                           .OrderByDescending(c => c.Id)
                           .FirstOrDefaultAsync();
                    if (lastAdmin != null) key = lastAdmin.UniqueKey;
                    int number = Convert.ToInt32(key[4..]);

                    Administrator admin = new Administrator
                    {
                        User = user,
                        UniqueKey = "adm-" + ++number
                    };
                    await _context.Administrators.AddAsync(admin);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("AdministratorsList", "Administrators");
                }
                else
                    ModelState.AddModelError(string.Empty, "A user with the same name already exists!");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> RemovingAdministrator(int? id)
        {
            if (id == null) return NotFound();
            
            Administrator admin = await _context.Administrators
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (admin == null) return NotFound();

            if (admin.User.Name.Equals(HttpContext.User.Identity.Name)) 
                return RedirectToAction("AdministratorsList", "Administrators");

            return View(admin);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingAdministrator(Administrator admin)
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == admin.UserId);
            if (user == null) return NotFound();

            _context.Administrators.Remove(admin);
            await _context.SaveChangesAsync();
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("AdministratorsList", "Administrators");
        }
    }
}
