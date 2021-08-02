using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class GenresController : Controller
    {
        private readonly DatabaseContext _context;

        public GenresController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GenresList(string search)
        {
            ViewBag.SearchData = search;

            var genres = _context.Genres
                .Select(g => g);

            if (!string.IsNullOrEmpty(search))
                genres = genres
                    .Where(g => g.Name.Contains(search));

            return View(await genres.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddingGenre() 
            => View();
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingGenre(Genre genre)
        {
            if (ModelState.IsValid)
            {
                await _context.Genres.AddAsync(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction("GenresList", "Genres");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GenreEditing(int? id)
        {
            if (id == null) return NotFound();
            
            Genre genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.Id == id);
            if (genre == null) return NotFound();

            return View(genre);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenreEditing(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Genres.Update(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction("GenresList", "Genres");
            }
            return RedirectToAction("GenreEditing", "Genres", new { id = genre.Id });
        }

        [HttpGet]
        public async Task<IActionResult> RemovingGenre(int? id)
        {
            if (id == null) return NotFound();

            Genre genre = await _context.Genres
                .FirstOrDefaultAsync(g => g.Id == id);
            if (genre == null) return NotFound();

            return View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingGenre(Genre genre)
        {
            var filters = await _context.Filters
                .Where(f => f.GenreId == genre.Id)
                .ToListAsync();
            _context.Filters.RemoveRange(filters);
            await _context.SaveChangesAsync();

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return RedirectToAction("GenresList", "Genres");
        }
    }
}
