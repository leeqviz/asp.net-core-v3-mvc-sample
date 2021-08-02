using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Storages;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class StoragesController : Controller
    {
        private readonly DatabaseContext _context;

        public StoragesController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> StoragesList(string search)
        {
            ViewBag.SearchData = search;

            var storages = _context.Storages
                .Include(s => s.Company)
                .Select(s => new StorageRecordViewModel
                {
                    Storage = s,
                    Employees = _context.Employees.Include(e => e.User).Where(e => e.StorageId == s.Id).ToList()
                });

            if (!string.IsNullOrEmpty(search))
                storages = storages
                    .Where(s => s.Storage.Name.Contains(search) || s.Storage.Company.Name.Contains(search));

            return View(await storages.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> AddingStorage()
        {
            var companies = await _context.Companies.ToListAsync();
            ViewBag.Companies = new SelectList(companies, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingStorage(Storage storage)
        {
            if (ModelState.IsValid)
            {
                await _context.Storages.AddAsync(storage);
                await _context.SaveChangesAsync();
                return RedirectToAction("StoragesList", "Storages");
            }
            return RedirectToAction("AddingStorage", "Storages");
        }

        [HttpGet]
        public async Task<IActionResult> StorageEditing(int? id)
        {
            if (id == null) return NotFound();
            
            Storage storage = await _context.Storages
                .FirstOrDefaultAsync(a => a.Id == id);
            if (storage == null) return NotFound();

            var companies = await _context.Companies.ToListAsync();
            ViewBag.Companies = new SelectList(companies, "Id", "Name");

            return View(storage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StorageEditing(Storage storage)
        {
            if (ModelState.IsValid)
            {
                _context.Storages.Update(storage);
                await _context.SaveChangesAsync();
                return RedirectToAction("StoragesList", "Storages");
            }
            return RedirectToAction("StorageEditing", "Storages");
        }

        [HttpGet]
        public async Task<IActionResult> RemovingStorage(int? id)
        {
            if (id == null) return NotFound();

            Storage storage = await _context.Storages
                .FirstOrDefaultAsync(a => a.Id == id);
            if (storage == null) return NotFound();

            bool copyExists = await _context.Copies
                .AnyAsync(c => c.StorageId == id);
            bool employeeExists = await _context.Employees
                .AnyAsync(e => e.StorageId == id);

            if (copyExists || employeeExists) 
                ViewBag.RecordExists = true;
            else ViewBag.RecordExists = false;

            return View(storage);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingStorage(Storage storage)
        {
            /*
            var copies = await _context.Copies
                .Where(c => c.StorageId == storage.Id)
                .ToListAsync();
            foreach (var copy in copies)
            {
                copy.StorageId = null;
                await _context.SaveChangesAsync();
            }

            var employees = await _context.Employees
                .Where(c => c.StorageId == storage.Id)
                .ToListAsync();
            foreach (var employee in employees)
            {
                employee.StorageId = null;
                await _context.SaveChangesAsync();
            }
            */
            _context.Storages.Remove(storage);
            await _context.SaveChangesAsync();
            return RedirectToAction("StoragesList", "Storages");
        }
    }
}
