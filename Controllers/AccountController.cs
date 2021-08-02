using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApp.Models.Account;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseContext _context;

        public AccountController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Name.Equals(model.Name) && u.Password.Equals(model.Password));
                if (user != null)
                {
                    await Authenticate(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, "Такой пользователь не существует!");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Name.Equals(model.Name));
                if (user == null)
                {
                    user = new User 
                    { 
                        Name = model.Name, 
                        Email = model.Email, 
                        Password = model.Password, 
                        ImagePath = "~/src/img/profiles/default.png", // default value
                        RegistrationDate = DateTime.Now
                    };
                    Role role = await _context.Roles
                        .FirstOrDefaultAsync(r => r.Name.Equals("client"));
                    if (role != null)
                    {
                        user.Role = role;
                        await _context.Users.AddAsync(user);
                        await _context.SaveChangesAsync();

                        string key = string.Empty;
                        Client lastClient = await _context.Clients
                               .OrderByDescending(c => c.Id).FirstOrDefaultAsync();
                        if (lastClient != null) key = lastClient.UniqueKey;
                        else key = "cln-1";

                        int number = Convert.ToInt32(key[4..]);
                        Client client = new Client
                        {
                            User = user,
                            UniqueKey = "cln-" + ++number
                        };
                        await _context.Clients.AddAsync(client);
                        await _context.SaveChangesAsync();

                        await Authenticate(user);
                        return RedirectToAction("Index", "Home");
                    }
                    else ModelState.AddModelError(string.Empty, "Возникла ошибка на стороне сервера!");
                }
                else
                    ModelState.AddModelError(string.Empty, "Пользователь с таким логином уже существует!");
            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name),
                new Claim("ProfileImage", user.ImagePath) //profile picture
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
