using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Catalog.Models.Content;
using WebApp.Models;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class ContentController : Controller
    {
        private readonly DatabaseContext _context;

        public ContentController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AllBooks(IEnumerable<int> genresIds, string search, string location, string sort, string type, int page = 1)
        {
            ViewBag.GenresIds = genresIds;
            ViewBag.GenresList = await _context.Genres.ToListAsync();

            var copies = _context.Copies
                .Include(c => c.Storage)
                .Join(_context.Companies, s => s.Storage.CompanyId, c => c.Id, (s, c) => new
                {
                    c.Type,
                    s.PublicationId,
                    s.Count,
                    s.Status
                });

            var books = _context.Publications
                .Include(p => p.Publisher)
                .Join(copies.Where(c => c.Count > 0 && c.Status.Equals("available")), p => p.Id, c => c.PublicationId, (p, c) => new BookViewModel
                {
                    PublisherName = p.Publisher.Name,
                    PublicationId = p.Id,
                    PublicationName = p.Name,
                    ImagePath = p.ImagePath,
                    Type = p.Type,
                    Year = p.Year,
                    CompanyType = c.Type
                });

            if (!string.IsNullOrEmpty(search)) 
                books = books.Where(b => b.PublicationName.Contains(search) || b.PublisherName.Contains(search));
            

            if (genresIds.Count() != 0)
            {
                var filters = _context.Filters.Where(f => genresIds.Contains((int)f.GenreId));
                books = books
                    .Join(filters, b => b.PublicationId, f => f.PublicationId, (b, f) => b);
            }

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.Equals("Название")) 
                    books = books.OrderByDescending(b => b.PublicationName);
                else if (sort.Equals("Издатель")) 
                    books = books.OrderByDescending(b => b.PublisherName);
            }

            if (!string.IsNullOrEmpty(location))
            {
                if (location.Equals("Библиотека")) 
                    books = books.Where(b => b.CompanyType.Equals("library"));
                else if (location.Equals("Магазин"))
                    books = books.Where(b => b.CompanyType.Equals("store"));
            }

            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("Бумажное"))
                    books = books.Where(b => b.Type.Equals("paper"));
                else if (type.Equals("Электронное"))
                    books = books.Where(b => b.Type.Equals("electronic"));
            }

            int pageSize = 8;
            var count = await books.CountAsync();
            var items = await books
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Distinct()
                .ToListAsync();

            ContentViewModel model = new ContentViewModel
            {
                Books = items,
                PageViewModel = new PageViewModel(count, page, pageSize),
                GenresIds = genresIds,
                Search = search,
                Sort = sort,
                Location = location,
                Type = type
            };

            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> AuthorDetail(int? id)
        {
            if (id == null) return NotFound();
            Author author = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);
            if (author == null) return NotFound();

            var literatures = await _context.Literatures
                .Where(l => l.AuthorId == author.Id).ToListAsync();

            AuthorDetailViewModel model = new AuthorDetailViewModel
            {
                Author = author,
                Literatures = literatures
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> BookDetail(int? id)
        {
            if (id == null) return NotFound();
            Publication publication = await _context.Publications
                .FirstOrDefaultAsync(p => p.Id == id);
            if (publication == null) return NotFound();

            var literatures = await _context.Literatures
                .Include(l => l.Author)
                .Join(_context.Books.Where(b => b.PublicationId == publication.Id), l => l.Id, b => b.LiteratureId, (l, b) => new LiteratureViewModel
                {
                    AuthorId = l.Author.Id,
                    AuthorName = l.Author.Name,
                    Literatures = _context.Literatures.Where(p => p.AuthorId == l.AuthorId && p.Id == b.LiteratureId).ToList()
                }).Distinct().ToListAsync();

            var locations = await _context.Copies
                .Where(c => c.Status.Equals("available") && c.PublicationId == publication.Id)
                .Include(c => c.Storage)
                .Join(_context.Companies, s => s.Storage.CompanyId, c => c.Id, (s, c) => new LocationViewModel
                {
                    Copy = s,
                    Storage = s.Storage,
                    Company = c
                }).ToListAsync();

            BookDetailViewModel model = new BookDetailViewModel
            {
                Publication = publication,
                Locations = locations,
                Literatures = literatures
            };

            // comments only for clients
            
            if (HttpContext.User.IsInRole("client"))
            {
                User user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
                if (user == null) return NotFound();

                Client client = await _context.Clients
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);
                if (client == null) return NotFound();

                History personalHistory = await _context.Histories
                    .FirstOrDefaultAsync(h => h.PublicationId == publication.Id && h.ClientId == client.Id);

                var histories = await _context.Histories
                    .Where(h => h.PublicationId == publication.Id && h.ClientId != client.Id)
                    .Join(_context.Clients.Include(c => c.User), h => h.ClientId, c => c.Id, (h, c) => new HistoryViewModel
                    {
                        UserName = c.User.Name,
                        Comment = h.Comment,
                        Rating = h.Rating,
                        UserProfileImage = c.User.ImagePath
                    }).ToListAsync();

                model.Histories = histories;
                model.PersonalHistory = personalHistory;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddHistory(int? id, int? rating, string comment)
        {
            if (id == null) return NotFound();

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();
            
            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            History history = await _context.Histories
                .FirstOrDefaultAsync(h => h.ClientId == client.Id && h.PublicationId == id);

            if (history == null)
            {
                await _context.Histories.AddAsync(new History
                {
                    Comment = comment,
                    Rating = rating,
                    ClientId = client.Id,
                    PublicationId = id
                });
            }
            else
            {
                history.Comment = comment;
                history.Rating = rating;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("BookDetail", "Content", new { id = id });
        }

        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> GetBook(int? id)
        {
            if (id == null) return NotFound();

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            Copy copy = await _context.Copies
                .FirstOrDefaultAsync(c => c.Id == id);
            if (copy == null) return NotFound();
            if (copy.Count < 1) return NotFound();
            /*
            Operation operation = await _context.Operations
                .FirstOrDefaultAsync(o => o.ClientId == client.Id && o.CopyId == id && o.Status.Equals("processing"));
            if (operation != null) return NotFound();
            */
            Cart cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.ClientId == client.Id && c.CopyId == id);
            if (cart != null)
            {
                if (cart.Count > 1)
                {
                    --cart.Count;
                }
                else
                {
                    _context.Carts.Remove(cart);
                }
                await _context.SaveChangesAsync();
            }

            await _context.Operations.AddAsync(new Operation
            {
                ClientId = client.Id,
                CopyId = copy.Id,
                Status = "processing",
                Type = "receive",
                CreationDate = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("AllBooks", "Content");
        }

        
        [Authorize(Roles = "client")]
        [HttpGet]
        public async Task<IActionResult> OrderForm(int? id)
        {
            if (id == null) return NotFound();

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            Copy copy = await _context.Copies
                .FirstOrDefaultAsync(c => c.Id == id);
            if (copy == null) return NotFound();
            if (copy.Count < 1) return NotFound();

            OrderFormViewModel model = new OrderFormViewModel
            {
                ClientId = client.Id,
                CopyId = copy.Id,
                Address = client.Address,
                FIO = client.FIO,
                PhoneNumber = client.PhoneNumber
            };

            return View(model);
        }

        [Authorize(Roles = "client")]
        [HttpPost]
        public async Task<IActionResult> OrderForm(OrderFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                Cart cart = await _context.Carts
                    .FirstOrDefaultAsync(c => c.ClientId == model.ClientId && c.CopyId == model.CopyId);
                if (cart != null)
                {
                    if (cart.Count > 1)
                    {
                        --cart.Count;
                    }
                    else
                    {
                        _context.Carts.Remove(cart);
                    }
                    await _context.SaveChangesAsync();
                }

                Operation operation = new Operation
                {
                    ClientId = model.ClientId,
                    CopyId = model.CopyId,
                    CreationDate = DateTime.Now,
                    Status = "processing",
                    Type = "purchase",
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

                return RedirectToAction("AllBooks", "Content");
            }
            return RedirectToAction("OrderForm", "Content");
        }


        [Authorize(Roles = "client")]
        public async Task<IActionResult> AddBookToWishList(int? id)
        {
            if (id == null) return NotFound();

            Copy copy = await _context.Copies
                .FirstOrDefaultAsync(c => c.Id == id);
            if (copy == null) return NotFound();

            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));
            if (user == null) return NotFound();

            Client client = await _context.Clients
                .FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (client == null) return NotFound();

            Storage storage = await _context.Storages
                .Include(s => s.Company)
                .FirstOrDefaultAsync(s => s.Id == copy.StorageId);
            if (storage == null) return NotFound();

            Cart cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.CopyId == id && c.ClientId == client.Id);
            if (cart != null)
            {
                ++cart.Count;
            }
            else
            {
                await _context.Carts.AddAsync(new Cart
                {
                    ClientId = client.Id,
                    CopyId = copy.Id,
                    Count = 1
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("AllBooks", "Content");
        }
    }
}
