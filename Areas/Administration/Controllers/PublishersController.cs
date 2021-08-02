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
    public class PublishersController : Controller
    {
        private readonly DatabaseContext _context;

        public PublishersController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> PublishersList(string search)
        {
            ViewBag.SearchData = search;

            var publisher = _context.Publishers
                .Select(p => p);

            if (!string.IsNullOrEmpty(search))
                publisher = publisher
                    .Where(p => p.Name.Contains(search));

            return View(await publisher.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddingPublisher() 
            => View();
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingPublisher(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                await _context.Publishers.AddAsync(publisher);
                await _context.SaveChangesAsync();
                return RedirectToAction("PublishersList", "Publishers");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PublishersEditing(int? id)
        {
            if (id == null) return NotFound();
            
            Publisher publisher = await _context.Publishers
                .FirstOrDefaultAsync(p => p.Id == id);
            if (publisher == null) return NotFound();

            bool publicationExists = await _context.Publications
                .AnyAsync(p => p.PublisherId == id);

            if (publicationExists) ViewBag.RecordExists = true;
            else ViewBag.RecordExists = false;

            return View(publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishersEditing(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                _context.Publishers.Update(publisher);
                await _context.SaveChangesAsync();

                return RedirectToAction("PublishersList", "Publishers");
            }
            return RedirectToAction("PublishersEditing", "Publishers", new { id = publisher.Id });
        }

        [HttpGet]
        public async Task<IActionResult> RemovingPublishers(int? id)
        {
            if (id == null) return NotFound();

            Publisher publisher = await _context.Publishers
                .FirstOrDefaultAsync(p => p.Id == id);
            if (publisher == null) return NotFound();

            return View(publisher);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingPublishers(Publisher pub)
        {
            var publications = await _context
                .Publications.Where(f => f.PublisherId == pub.Id).ToListAsync();
            foreach (var i in publications)
            {
                i.PublisherId = null;
                await _context.SaveChangesAsync();
            }

            _context.Publishers.Remove(pub);
            await _context.SaveChangesAsync();

            return RedirectToAction("PublishersList", "Publishers");
        }
    }
}
