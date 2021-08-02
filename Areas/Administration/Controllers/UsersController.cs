using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Users;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> UsersList(string search)
        {
            ViewBag.SearchData = search;

            var users = _context.Users
                .Include(u => u.Role)
                .Select(u => new UserRecordViewModel
                {
                    UserId = u.Id,
                    UserName = u.Name,
                    Email = u.Email,
                    RoleName = u.Role.Name,
                    RegistrationDate = u.RegistrationDate,
                    ImagePath = u.ImagePath
                });

            if (!string.IsNullOrEmpty(search)) 
                users = users
                    .Where(u => u.UserName.Contains(search) || u.Email.Contains(search));
            
            return View(await users.ToListAsync());
        }
    }
}
