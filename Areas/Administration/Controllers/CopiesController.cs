using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Copies;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class CopiesController : Controller
    {
        private readonly DatabaseContext _context;

        public CopiesController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> LibraryCopiesList(string search)
        {
            ViewBag.SearchData = search;

            var copies = _context.Storages
                .Include(s => s.Company)
                .Where(s => s.Company.Type.Equals("library"))
                .Join(_context.Copies.Include(c => c.Publication), s => s.Id, c => c.StorageId, (s, c) => new CopyRecordViewModel
                {
                    PublicationId = c.Publication.Id,
                    PublicationName = c.Publication.Name,
                    Time = c.Time,
                    Count = c.Count,
                    Status = c.Status,
                    CompanyName = s.Company.Name,
                    CopyUniqueKey = c.UniqueKey,
                    EmployeeUniqueKey = _context.Employees.FirstOrDefault(e => e.Id == c.EmployeeId).UniqueKey,
                    ModifiedDate = c.ModifiedDate,
                    CopyId = c.Id
                }).Distinct();

            if (!string.IsNullOrEmpty(search))
                copies = copies
                    .Where(c => c.PublicationName.Contains(search) || c.CompanyName.Contains(search));

            return View(await copies.ToListAsync());
        }

        public async Task<IActionResult> StoreCopiesList(string search)
        {
            ViewBag.SearchData = search;

            var copies = _context.Storages
                .Include(s => s.Company)
                .Where(s => s.Company.Type.Equals("store"))
                .Join(_context.Copies.Include(c => c.Publication), s => s.Id, c => c.StorageId, (s, c) => new CopyRecordViewModel
                {
                    PublicationId = c.Publication.Id,
                    PublicationName = c.Publication.Name,
                    Price = c.Price,
                    Count = c.Count,
                    Status = c.Status,
                    CompanyName = s.Company.Name,
                    CopyUniqueKey = c.UniqueKey,
                    EmployeeUniqueKey = _context.Employees.FirstOrDefault(e => e.Id == c.EmployeeId).UniqueKey,
                    ModifiedDate = c.ModifiedDate,
                    CopyId = c.Id
                }).Distinct();

            if (!string.IsNullOrEmpty(search))
                copies = copies
                    .Where(c => c.PublicationName.Contains(search) || c.CompanyName.Contains(search));

            return View(await copies.ToListAsync());
        }

        /*
        add copy
        edit copy
        remove copy
        */
    }
}
