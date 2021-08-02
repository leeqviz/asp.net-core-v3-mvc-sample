using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Profile.Models.WishList;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Profile.Controllers
{
    [Area("Profile")]
    [Authorize(Roles = "client")]
    public class WishListController : Controller
    {
        private readonly DatabaseContext _context;

        public WishListController(DatabaseContext context)
        {
            _context = context;
        }

        private IEnumerable<WishListBookViewModel> GetCarts(int? id)
        {
            var copies = _context.Carts
                .Where(c => c.ClientId == id)
                .Include(c => c.Copy)
                .Join(_context.Storages.Include(s => s.Company), c => c.Copy.StorageId, s => s.Id, (c, s) => new
                {
                    c.Copy.Price,
                    c.Copy.Time,
                    c.Copy.Count,
                    s.Company.Name,
                    s.Company.Address,
                    s.Company.PhoneNumber,
                    s.Company.Type,
                    CountInCart = c.Count,
                    CartId = c.Id,
                    CopyId = c.Copy.Id,
                    publicationId = c.Copy.PublicationId
                });

            var result = copies
                .Join(_context.Publications, c => c.publicationId, p => p.Id, (c, p) => new WishListBookViewModel
                {
                    PublicationName = p.Name,
                    ImagePath = p.ImagePath,
                    PublicationId = p.Id,
                    CartId = c.CartId,
                    CopyId = c.CopyId,
                    CompanyAddress = c.Address,
                    CompanyName = c.Name,
                    CompanyPhoneNumber = c.PhoneNumber,
                    CompanyType = c.Type,
                    Count = c.Count,
                    CountInCart = c.CountInCart,
                    Price = c.Price,
                    Time = c.Time
                });

            return result;
        }

        public async Task<IActionResult> LibraryContentViewer(string search)
        {
            ViewBag.SearchData = search;

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            var result = GetCarts(client.Id);

            result = result
                .Where(r => r.CompanyType.Equals("library"));

            if (!string.IsNullOrEmpty(search))
                result = result
                    .Where(r => r.PublicationName.Contains(search) || r.CompanyName.Contains(search));

            return View(result.Distinct().ToList());
        }

        public async Task<IActionResult> StoreContentViewer(string search)
        {
            ViewBag.SearchData = search;

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            var result = GetCarts(client.Id);

            result = result
                .Where(r => r.CompanyType.Equals("store"));

            if (!string.IsNullOrEmpty(search))
                result = result
                    .Where(r => r.PublicationName.Contains(search) || r.CompanyName.Contains(search));

            return View(result.Distinct().ToList());
        }

        public async Task<IActionResult> RemoveBookFromWishList(int? id, string type)
        {
            if (id == null) return NotFound();

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            Cart cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == id && c.ClientId == client.Id);
            if (cart == null) return NotFound();
            else
            {
                if (cart.Count > 1) --cart.Count;
                else _context.Carts.Remove(cart);
            }
            await _context.SaveChangesAsync();

            if (type.Equals("library")) return RedirectToAction("LibraryContentViewer", "WishList");
            else return RedirectToAction("StoreContentViewer", "WishList");
        }

        public async Task<IActionResult> GetAllBooks()
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            var copies = _context.Copies
                .Join(_context.Storages
                .Include(s => s.Company)
                .Where(s => s.Company.Type.Equals("library")), c => c.StorageId, s => s.Id, (c, s) => c);

            var carts = await _context.Carts
                .Where(c => c.ClientId == client.Id)
                .Join(copies, cr => cr.CopyId, cp => cp.Id, (cr, cp) => cr)
                .Distinct()
                .ToListAsync();
            if (carts.Count() == 0) return NotFound();

            foreach (var cart in carts)
            {
                for(int i = 0; i < cart.Count; i++)
                {
                    await _context.Operations.AddAsync(new Operation
                    {
                        ClientId = cart.ClientId,
                        CopyId = cart.CopyId,
                        CreationDate = DateTime.Now,
                        Type = "receive",
                        Status = "processing"
                    });
                    await _context.SaveChangesAsync();
                }
            }

            _context.Carts.RemoveRange(carts);
            await _context.SaveChangesAsync();

            return RedirectToAction("LibraryContentViewer", "WishList");
        }

        [HttpGet]
        public async Task<IActionResult> BuyAllBooks()
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            var copies = _context.Copies
                .Join(_context.Storages
                .Include(s => s.Company)
                .Where(s => s.Company.Type.Equals("store")), c => c.StorageId, s => s.Id, (c, s) => c);

            var carts = await _context.Carts
                .Where(c => c.ClientId == client.Id)
                .Join(copies, cr => cr.CopyId, cp => cp.Id, (cr, cp) => cr)
                .Distinct()
                .ToListAsync();
            if (carts.Count() == 0) return NotFound();

            OrderingViewModel model = new OrderingViewModel
            {
                ClientId = client.Id,
                Address = client.Address,
                FIO = client.FIO,
                PhoneNumber = client.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BuyAllBooks(OrderingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var copies = _context.Copies
                .Join(_context.Storages
                .Include(s => s.Company)
                .Where(s => s.Company.Type.Equals("store")), c => c.StorageId, s => s.Id, (c, s) => c);

                var carts = await _context.Carts
                    .Where(c => c.ClientId == model.ClientId)
                    .Join(copies, cr => cr.CopyId, cp => cp.Id, (cr, cp) => cr)
                    .Distinct()
                    .ToListAsync();
                if (carts.Count() == 0) return NotFound();

                foreach (var cart in carts)
                {
                    for (int i = 0; i < cart.Count; i++)
                    {
                        Operation operation = new Operation
                        {
                            ClientId = cart.ClientId,
                            CopyId = cart.CopyId,
                            CreationDate = DateTime.Now,
                            Type = "receive",
                            Status = "processing"
                        };
                        await _context.Operations.AddAsync(operation);
                        await _context.SaveChangesAsync();

                        Order order = new Order
                        {
                            Operation = operation,
                            Address = model.Address,
                            FIO = model.FIO,
                            PhoneNumber = model.PhoneNumber
                        };
                        await _context.Orders.AddAsync(order);
                        await _context.SaveChangesAsync();
                    }
                }

                _context.Carts.RemoveRange(carts);
                await _context.SaveChangesAsync();

                return RedirectToAction("StoreContentViewer", "WishList");
            }
            return RedirectToAction("BuyAllBooks", "WishList");
        }
    }
}
