using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Authors;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class AuthorsController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public AuthorsController(DatabaseContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> AuthorsList(string search)
        {
            ViewBag.SearchData = search;

            var authors = _context.Authors
                .Select(a => new AuthorRecordViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    ImagePath = a.ImagePath,
                    Literatures = _context.Literatures
                        .Where(l => l.AuthorId == a.Id)
                        .ToList()
                });

            if (!string.IsNullOrEmpty(search))
                authors = authors
                    .Where(a => a.Name.Contains(search));

            return View(await authors.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddingAuthor() 
            => View();
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingAuthor(NewAuthorRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // check image
                string image = null;
                if (model.PreviewImage != null)
                {
                    string path = "/src/img/authors/" + model.PreviewImage.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.PreviewImage.CopyToAsync(fileStream);
                    }
                    image = "~" + path;
                }

                await _context.Authors.AddAsync(new Author
                {
                    ImagePath = image,
                    Description = model.Description,
                    Name = model.Name
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("AuthorsList", "Authors");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AuthorEditing(int? id)
        {
            if (id == null) return NotFound();
            
            Author author = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)  return NotFound();

            AuthorDataRecordViewModel model = new AuthorDataRecordViewModel
            {
                Author = author,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AuthorEditing(AuthorDataRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // check image
                string image = null;
                if (model.PreviewImage != null)
                {
                    string path = "/src/img/authors/" + model.PreviewImage.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.PreviewImage.CopyToAsync(fileStream);
                    }
                    image = "~" + path;
                }

                model.Author.ImagePath = image;
                _context.Authors.Update(model.Author);
                await _context.SaveChangesAsync();
                return RedirectToAction("AuthorsList", "Authors");
            }
            return RedirectToAction("AuthorEditing", "Authors", new { id = model.Author.Id });
        }

        [HttpGet]
        public async Task<IActionResult> RemovingAuthor(int? id)
        {
            if (id == null) return NotFound();

            Author author = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);
            if (author == null) return NotFound();

            bool literatureExists = await _context.Literatures
                .AnyAsync(s => s.AuthorId == id);

            if (literatureExists)
                ViewBag.RecordExists = true;
            else ViewBag.RecordExists = false;

            return View(author);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingAuthor(Author author)
        {
            /*
            var literatures = await _context.Literatures
                .Where(f => f.AuthorId == author.Id).ToListAsync();
            _context.Literatures.RemoveRange(literatures);
            */
            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction("AuthorsList", "Authors");
        }

        [HttpGet]
        public async Task<IActionResult> AddingLiterature(int? authorId)
        {
            if (authorId == null) return NotFound();
            
            Author author = await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == authorId);
            if (author == null) return NotFound();

            ViewBag.AuthorId = author.Id;
            ViewBag.AuthorName = author.Name;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingLiterature(Literature literature)
        {
            if (ModelState.IsValid)
            {
                await _context.Literatures.AddAsync(new Literature 
                { 
                    AuthorId = literature.AuthorId,
                    Name = literature.Name
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("AuthorsList", "Authors");
            }
            return RedirectToAction("AddingLiterature", "Authors", new { authorId = literature.AuthorId });
        }

        [HttpGet]
        public async Task<IActionResult> LiteratureEditing(int? id)
        {
            if (id == null) return NotFound();

            Literature literature = await _context.Literatures
                .Include(l => l.Author)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (literature == null) return NotFound();

            return View(literature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LiteratureEditing(Literature literature)
        {
            if (ModelState.IsValid)
            {
                _context.Literatures.Update(literature);
                await _context.SaveChangesAsync();
                return RedirectToAction("AuthorsList", "Authors");
            }
            return RedirectToAction("LiteratureEditing", "Authors", new { id = literature.Id });
        }

        [HttpGet]
        public async Task<IActionResult> RemovingLiterature(int? id)
        {
            if (id == null) return NotFound();
            
            Literature literature = await _context.Literatures
                .Include(l => l.Author)
                .FirstOrDefaultAsync(l => l.Id == id);
            if (literature == null) return NotFound();

            bool booksExists = await _context.Books
                .AnyAsync(b => b.LiteratureId == id);

            if (booksExists)
                ViewBag.RecordExists = true;
            else ViewBag.RecordExists = false;

            return View(literature);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingLiterature(Literature literature)
        {
            var books = await _context.Books
                .Where(b => b.LiteratureId == literature.Id)
                .ToListAsync();
            _context.Books.RemoveRange(books);
            await _context.SaveChangesAsync();

            _context.Literatures.Remove(literature);
            await _context.SaveChangesAsync();

            return RedirectToAction("AuthorsList", "Authors");
        }
    }
}
