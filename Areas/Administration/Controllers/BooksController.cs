using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Books;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class BooksController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public BooksController(DatabaseContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> BooksList(string search)
        {
            ViewBag.SearchData = search;

            var publications = _context.Publications
                .Include(p => p.Publisher);

            var books = _context.Books
                .Select(b => new 
                {
                    b.Id,
                    b.PublicationId,
                    Literatures = _context.Literatures
                        .Include(l => l.Author)
                        .Where(l => l.Id == b.LiteratureId)
                        .ToList()
                });

            var filters = _context.Filters
                .Select(f => new
                {
                    f.Id,
                    f.PublicationId,
                    Genres = _context.Genres
                        .Where(g => g.Id == f.GenreId)
                        .ToList()
                });

            //union with nullable fks

            var result = (from p in publications
                            join b in books on p.Id equals b.PublicationId into d
                            from m in d.DefaultIfEmpty()
                            select new BookRecordViewModel
                            {
                                Publication = p,
                                Literatures = m.Literatures,
                            });

            result = (from r in result
                            join f in filters on r.Publication.Id equals f.PublicationId into d
                            from m in d.DefaultIfEmpty()
                            select new BookRecordViewModel
                            {
                                Publication = r.Publication,
                                Literatures = r.Literatures,
                                Genres = m.Genres
                            });

            if (!string.IsNullOrEmpty(search))
                result = result
                    .Where(r => r.Publication.Name.Contains(search));

            return View(await result.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddingBook()
        {
            var publishers = await _context.Publishers.ToListAsync();
            ViewBag.PublishersList = new SelectList(publishers, "Id", "Name");

            var genres = await _context.Genres.ToListAsync();
            ViewBag.GenresList = new SelectList(genres, "Id", "Name");

            var literatures = await _context.Literatures
                .Include(l => l.Author)
                .ToListAsync();
            var lList = new List<object>();
            foreach (var literature in literatures)
            {
                lList.Add(new
                {
                    Id = literature.Id,
                    Name = literature.Author.Name + " | " + literature.Name
                });
            }
            ViewBag.LiteraturesList = new SelectList(lList, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingBook(NewBookRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // check image
                string image = null;
                if (model.PreviewImage != null)
                {
                    string path = "/src/img/books/" + model.PreviewImage.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.PreviewImage.CopyToAsync(fileStream);
                    }
                    image = "~" + path;
                }

                string type = string.Empty;
                if (model.Type.Equals("Электронное"))
                {
                    type = "electronic";
                }
                else if (model.Type.Equals("Бумажное"))
                {
                    type = "paper";
                }

                Publication publication = new Publication
                {
                    Name = model.Name,
                    Description = model.Description,
                    Type = type,
                    Year = model.Year,
                    PublisherId = model.PublisherId,
                    ImagePath = image
                };
                await _context.Publications.AddAsync(publication);
                await _context.SaveChangesAsync();

                List<Filter> filters = new List<Filter>();
                foreach (var item in model.Genres)
                {
                    filters.Add(new Filter
                    {
                        GenreId = item,
                        PublicationId = publication.Id
                    });
                }
                await _context.Filters.AddRangeAsync(filters);

                List<Book> books = new List<Book>();
                foreach (var item in model.Literatures)
                {
                    books.Add(new Book
                    {
                        LiteratureId = item,
                        PublicationId = publication.Id
                    });
                }
                await _context.Books.AddRangeAsync(books);
                await _context.SaveChangesAsync();

                return RedirectToAction("BooksList", "Books");
            }
            return RedirectToAction("AddingBook", "Books");
        }

        [HttpGet]
        public async Task<IActionResult> BookEditing(int? id)
        {
            if (id != null)
            {
                var publishers = await _context.Publishers.ToListAsync();
                ViewBag.PublishersList = new SelectList(publishers, "Id", "Name");

                var genres = await _context.Genres.ToListAsync();
                ViewBag.GenresList = new SelectList(genres, "Id", "Name");

                var literatures = await _context.Literatures.Include(l => l.Author).ToListAsync();
                var lList = new List<object>();
                foreach (var literature in literatures)
                {
                    lList.Add(new
                    {
                        Id = literature.Id,
                        Name = literature.Author.Name + " | " + literature.Name
                    });
                }
                ViewBag.LiteraturesList = new SelectList(lList, "Id", "Name");

                Publication publication = await _context.Publications.FirstOrDefaultAsync(p => p.Id == id);
                BookDataRecordViewModel bvm = new BookDataRecordViewModel
                {
                    Publication = publication
                };
                if (publication != null) return View(bvm);
                else return NotFound();
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookEditing(BookDataRecordViewModel bvm)
        {
            if (ModelState.IsValid)
            {
                var rFilters = await _context.Filters
                    .Where(f => f.PublicationId == bvm.Publication.Id)
                    .ToListAsync();
                _context.Filters.RemoveRange(rFilters);
                await _context.SaveChangesAsync();

                List<Filter> filters = new List<Filter>();
                foreach (var item in bvm.Genres)
                {
                    filters.Add(new Filter
                    {
                        GenreId = item,
                        PublicationId = bvm.Publication.Id
                    });
                }
                await _context.Filters.AddRangeAsync(filters);
                await _context.SaveChangesAsync();

                var rBooks = await _context.Books
                    .Where(f => f.PublicationId == bvm.Publication.Id)
                    .ToListAsync();
                _context.Books.RemoveRange(rBooks);
                await _context.SaveChangesAsync();

                List<Book> books = new List<Book>();
                foreach (var item in bvm.Literatures)
                {
                    books.Add(new Book
                    {
                        LiteratureId = item,
                        PublicationId = bvm.Publication.Id
                    });
                }
                await _context.Books.AddRangeAsync(books);
                await _context.SaveChangesAsync();

                string image = null;
                if (bvm.PreviewImage != null)
                {
                    string path = "/src/img/books/" + bvm.PreviewImage.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await bvm.PreviewImage.CopyToAsync(fileStream);
                    }
                    image = "~" + path;
                }
                bvm.Publication.ImagePath = image;

                if (bvm.Publication.Type.Equals("Электронное"))
                {
                    bvm.Publication.Type = "electronic";
                }
                else if (bvm.Publication.Type.Equals("Бумажное"))
                {
                    bvm.Publication.Type = "paper";
                }

                _context.Publications.Update(bvm.Publication);

                await _context.SaveChangesAsync();
                return RedirectToAction("BooksList", "Books");
            }
            return RedirectToAction("BookEditing", "Books");
        }

        [HttpGet]
        public async Task<IActionResult> RemovingBook(int? id)
        {
            if (id == null) return NotFound();
            
            Publication publication = await _context.Publications
                .FirstOrDefaultAsync(p => p.Id == id);
            if (publication == null) return NotFound();

            bool copyExists = await _context.Copies
                .AnyAsync(c => c.PublicationId == id);

            bool historyExists = await _context.Histories
                .AnyAsync(c => c.PublicationId == id);

            if (copyExists || historyExists)
                ViewBag.RecordExists = true;
            else ViewBag.RecordExists = false;

            return View(publication);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingBook(Publication publication)
        {
            /*
            var copies = await _context.Copies.Where(c => c.PublicationId == publication.Id).ToListAsync();
            foreach (var copy in copies)
            {
                copy.PublicationId = null;
                await _context.SaveChangesAsync();
            }
            */
            var rFilters = await _context.Filters.Where(f => f.PublicationId == publication.Id).ToListAsync();
            _context.Filters.RemoveRange(rFilters);
            await _context.SaveChangesAsync();

            var rBooks = await _context.Books.Where(f => f.PublicationId == publication.Id).ToListAsync();
            _context.Books.RemoveRange(rBooks);
            await _context.SaveChangesAsync();

            _context.Publications.Remove(publication);
            await _context.SaveChangesAsync();
            return RedirectToAction("BooksList", "Books");
        }
    }
}
