using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Management.Models.CopiesEditor;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = "employee")]
    public class ExemplarsController : Controller
    {
        private readonly DatabaseContext _context;

        public ExemplarsController(DatabaseContext context)
        {
            _context = context;
        }

        private async Task<Employee> GetEmployee()
        {
            User user = await _context.Users
                .FirstOrDefaultAsync(u => u.Name.Equals(HttpContext.User.Identity.Name));

            return await _context.Employees
                .FirstOrDefaultAsync(e => e.UserId == user.Id);
        }
        
        private IQueryable<CopyViewModel> GetCopies(string status, int? storageId)
        {
            var copies = _context.Copies
                .Where(c => c.Status.Equals(status) && c.StorageId == storageId);

            var publications = _context.Publications
                .Include(p => p.Publisher)
                .Select(p => new
                {
                    PublicationId = p.Id,
                    PublicationName = p.Name,
                    PublisherName = p.Publisher.Name
                });

            var result = copies
                .Join(publications, c => c.PublicationId, p => p.PublicationId, (c, p) => new CopyViewModel
                {
                    PublisherName = p.PublisherName,
                    PublicationId = p.PublicationId,
                    PublicationName = p.PublicationName,
                    Count = c.Count,
                    Time = c.Time,
                    Price = c.Price,
                    CopyUniqueKey = c.UniqueKey,
                    ModifiedDate = c.ModifiedDate,
                    CopyId = c.Id
                }).Distinct();

            return result;
        }

        public async Task<IActionResult> AvailableCopies(string search) 
        {
            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();
            if (employee.StorageId != null)
            {
                Storage storage = await _context.Storages
                    .FirstOrDefaultAsync(s => s.Id == employee.StorageId);
                if (storage == null) return NotFound();

                Company company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.Id == storage.CompanyId);
                if (company == null) return NotFound();

                ViewBag.CompanyType = company.Type;
                ViewBag.SearchData = search;

                var copies = GetCopies("available", employee.StorageId);

                if (!string.IsNullOrEmpty(search))
                    copies = copies
                        .Where(c => c.PublicationName.Contains(search) || c.PublisherName.Contains(search));

                return View(await copies.ToListAsync());
            }
            else return NotFound();
        }

        public async Task<IActionResult> WrittenOffCopies(string search) 
        {
            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();
            if (employee.StorageId != null)
            {
                Storage storage = await _context.Storages
                    .FirstOrDefaultAsync(s => s.Id == employee.StorageId);
                if (storage == null) return NotFound();

                Company company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.Id == storage.CompanyId);
                if (company == null) return NotFound();

                ViewBag.CompanyType = company.Type;
                ViewBag.SearchData = search;

                var copies = GetCopies("written-off", employee.StorageId);

                if (!string.IsNullOrEmpty(search))
                    copies = copies
                        .Where(c => c.PublicationName.Contains(search) || c.PublisherName.Contains(search));

                return View(await copies.ToListAsync());
            }
            else return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> WriteOffCopy(int? id)
        {
            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();

            Copy copy = await _context.Copies
                .FirstOrDefaultAsync(c => c.Id == id);
            if (copy == null) return NotFound();

            copy.Status = "written-off";
            copy.EmployeeId = employee.Id;
            copy.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("AvailableCopies", "Exemplars");
        }

        [HttpGet]
        public async Task<IActionResult> ResumeCopy(int? id)
        {
            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();

            Copy copy = await _context.Copies
                .FirstOrDefaultAsync(c => c.Id == id);
            if (copy == null) return NotFound();

            copy.Status = "available";
            copy.EmployeeId = employee.Id;
            copy.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("WrittenOffCopies", "Exemplars");
        }
        
        [HttpGet]
        public async Task<IActionResult> AddingCopy()
        {
            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();
            if (employee.StorageId != null)
            {
                Storage storage = await _context.Storages
                    .FirstOrDefaultAsync(s => s.Id == employee.StorageId);
                if (storage == null) return NotFound();

                Company company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.Id == storage.CompanyId);
                if (company == null) return NotFound();

                ViewBag.CompanyType = company.Type;
            }
            else return NotFound();

            var publications = await _context.Publications
                .Include(p => p.Publisher)
                .Select(p => new
                {
                    PublicationId = p.Id,
                    PublicationName = p.Name,
                    PublisherName = p.Publisher.Name
                }).ToListAsync();

            List<object> pList = new List<object>();
            foreach (var p in publications)
            {
                pList.Add(new
                {
                    Id = p.PublicationId,
                    Name = p.PublicationName + " | " + p.PublisherName
                });
            }
            ViewBag.Publications = new SelectList(pList, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingCopy(NewCopyViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = await GetEmployee();
                if (employee == null || employee.StorageId == null) return NotFound();

                Copy record = new Copy
                {
                    Status = "available",
                    StorageId = employee.StorageId,
                    ModifiedDate = DateTime.Now,
                    Count = model.Count,
                    Price = model.Price == null ? 0.0f : model.Price,
                    Time = model.Time == null ? 14 : model.Time,
                    PublicationId = model.PublicationId,
                    EmployeeId = employee.Id
                };

                Copy copy = await _context.Copies
                    .FirstOrDefaultAsync(c => c.StorageId == employee.StorageId && c.PublicationId == record.PublicationId);
                if (copy != null)
                {
                    copy.Count += record.Count;
                    copy.Price = record.Price;
                    copy.Time = record.Time;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    string key = "cpy-0";
                    Copy lastCopy = await _context.Copies
                           .OrderByDescending(c => c.Id)
                           .FirstOrDefaultAsync();
                    if (lastCopy != null) key = lastCopy.UniqueKey;
                    int number = Convert.ToInt32(key[4..]);

                    record.UniqueKey = "cpy-" + ++number;

                    await _context.Copies.AddAsync(record);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("AvailableCopies", "Exemplars");
            }
            return RedirectToAction("AddingCopy", "Exemplars");
        }

        [HttpGet]
        public async Task<IActionResult> CopyEditing(int? id)
        {
            if (id != null)
            {
                Copy copy = await _context.Copies
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (copy != null) 
                {
                    Employee employee = await GetEmployee();
                    if (employee == null) return NotFound();
                    if (employee.StorageId != null)
                    {
                        Storage storage = await _context.Storages
                            .FirstOrDefaultAsync(s => s.Id == employee.StorageId);
                        if (storage == null) return NotFound();

                        Company company = await _context.Companies
                            .FirstOrDefaultAsync(c => c.Id == storage.CompanyId);
                        if (company == null) return NotFound();

                        ViewBag.CompanyType = company.Type;
                    }
                    else return NotFound();

                    var publications = await _context.Publications
                        .Include(p => p.Publisher)
                        .Select(p => new
                        {
                            PublicationId = p.Id,
                            PublicationName = p.Name,
                            PublisherName = p.Publisher.Name
                        }).ToListAsync();

                    List<object> pList = new List<object>();
                    foreach (var p in publications)
                    {
                        pList.Add(new
                        {
                            Id = p.PublicationId,
                            Name = p.PublicationName + " | " + p.PublisherName
                        });
                    }
                    ViewBag.Publications = new SelectList(pList, "Id", "Name");

                    CopyDataViewModel model = new CopyDataViewModel
                    {
                        Count = copy.Count,
                        Price = copy.Price,
                        Time = copy.Time,
                        CopyId = copy.Id,
                        PublicationId = copy.PublicationId
                    };
                    return View(model); 
                }
                else return NotFound();
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CopyEditing(CopyDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                Copy copy = await _context.Copies
                    .FirstOrDefaultAsync(c => c.Id == model.CopyId);
                if (copy == null) return NotFound();

                copy.Count = model.Count;
                copy.Price = model.Price == null ? 0.0f : model.Price;
                copy.Time = model.Time == null ? 14 : model.Time;
                copy.PublicationId = model.PublicationId;
                await _context.SaveChangesAsync();
                return RedirectToAction("AvailableCopies", "Exemplars");
            }
            return RedirectToAction("CopyEditing", "Exemplars");
        }

        [HttpGet]
        public async Task<IActionResult> RemovingCopy(int? id)
        {
            if (id == null) return NotFound();
            
            Copy copy = await _context.Copies
                .FirstOrDefaultAsync(p => p.Id == id);
            if (copy == null) return NotFound();

            bool cartExists = await _context.Carts
                .AnyAsync(c => c.CopyId == id);

            bool operationExists = await _context.Operations
                .AnyAsync(c => c.CopyId == id);

            if (cartExists || operationExists)
                ViewBag.RecordExists = true;
            else ViewBag.RecordExists = false;

            return View(copy);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingCopy(Copy copy)
        {
            /*
            var operations = await _context.Operations
                .Where(f => f.CopyId == copy.Id).ToListAsync();
            foreach (var i in operations)
            {
                i.CopyId = null;
                await _context.SaveChangesAsync();
            }

            var carts = await _context.Carts
                .Where(f => f.CopyId == copy.Id).ToListAsync();
            _context.Carts.RemoveRange(carts);
            await _context.SaveChangesAsync();
            */
            _context.Copies.Remove(copy);
            await _context.SaveChangesAsync();
            return RedirectToAction("AvailableCopies", "Exemplars");
        }
    }
}
