using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Areas.Profile.Models.PersonalData;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Authorize]
    public class PersonalDataController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public PersonalDataController(DatabaseContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        private async Task<User> GetCurrentUser() => await _context.Users
            .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));

        public async Task<IActionResult> ProfileViewer()
        {
            User user = await GetCurrentUser();
            Role role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == user.RoleId);
            ProfileDataViewModel model = new ProfileDataViewModel
            {
                User = user,
                Client = null,
                Employee = null,
                Role = role
            };
            if (role.Name.Equals("admin"))
                model.Administrator = await _context.Administrators
                    .FirstOrDefaultAsync(a => a.UserId == user.Id);

            else if (role.Name.Equals("employee"))
                model.Employee = await _context.Employees
                    .FirstOrDefaultAsync(e => e.UserId == user.Id);

            else if (role.Name.Equals("client"))
                model.Client = await _context.Clients
                    .Include(c => c.Category)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

            else
            {
                // just a user
            }
            return View(model);
        }

        private async Task UpdateClaims(User user)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim("ProfileImage", user.ImagePath) //profile picture
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> ProfileEditor()
        {
            User user = await GetCurrentUser();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            var categories = await _context.Categories.ToListAsync();
            ViewBag.CategoriesList = new SelectList(categories, "Id", "Name");

            DataViewModel model = new DataViewModel
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                ProfileImage = user.ImagePath,
                FIO = client.FIO,
                Age = client.Age,
                Address = client.Address,
                PhoneNumber = client.PhoneNumber,
                CategoryId = client.CategoryId
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileEditor(DataViewModel model)
        {
            if (ModelState.IsValid)
            {
                User sameUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Name.Equals(model.Name));
                User user = await GetCurrentUser();
                if (sameUser == null || (sameUser != null && sameUser.Id == user.Id))
                {
                    // save img
                    if (model.Avatar != null)
                    {
                        string path = "/src/img/profiles/" + model.Avatar.FileName;
                        using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                        {
                            await model.Avatar.CopyToAsync(fileStream);
                        }
                        model.ProfileImage = "~" + path;
                    }
                    user.Name = model.Name;
                    user.Email = model.Email;
                    user.Password = model.Password;
                    user.ImagePath = model.ProfileImage;

                    if (user.Role == null)
                        user.Role = await _context.Roles
                            .FirstOrDefaultAsync(r => r.Id == user.RoleId);
                    await _context.SaveChangesAsync();

                    Client client = await _context.Clients
                        .FirstOrDefaultAsync(c => c.UserId == user.Id);
                    client.Address = model.Address;
                    client.Age = model.Age;
                    client.FIO = model.FIO;
                    client.PhoneNumber = model.PhoneNumber;
                    client.CategoryId = model.CategoryId;
                    await _context.SaveChangesAsync();

                    await UpdateClaims(user);
                }
                else
                    ModelState.AddModelError(string.Empty, "Пользователь с таким логином уже существует!");

                return RedirectToAction("ProfileViewer", "PersonalData");
            }
            return View(model);
        }
    }
}
