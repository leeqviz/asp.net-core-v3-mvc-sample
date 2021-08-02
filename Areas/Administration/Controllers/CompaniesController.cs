using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Companies;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class CompaniesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public CompaniesController(DatabaseContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> CompaniesList(string search)
        {
            ViewBag.SearchData = search;

            var companies = _context.Companies
                .Select(c => new CompanyRecordViewModel
                {
                    Company = c,
                    Storages = _context.Storages.Where(s => s.CompanyId == c.Id).ToList()
                });

            if (!string.IsNullOrEmpty(search))
                companies = companies
                    .Where(c => c.Company.Name.Contains(search) || c.Company.Address.Contains(search));

            return View(await companies.ToListAsync());
        }

        [HttpGet]
        public IActionResult AddingCompany() 
            => View();
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddingCompany(NewCompanyRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // check image
                if (model.PreviewImage != null)
                {
                    string path = "/src/img/companies/" + model.PreviewImage.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.PreviewImage.CopyToAsync(fileStream);
                    }
                    model.ImagePath = "~" + path;
                }

                string type = string.Empty;
                if (model.Type.Equals("Библиотека"))
                {
                    type = "library";
                }
                else if (model.Type.Equals("Магазин"))
                {
                    type = "store";
                }

                await _context.Companies.AddAsync(new Company
                {
                    Address = model.Address,
                    Description = model.Description,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Type = type,
                    ImagePath = model.ImagePath
                });
                await _context.SaveChangesAsync();
                return RedirectToAction("CompaniesList", "Companies");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CompanyEditing(int? id)
        {
            if (id == null) return NotFound();
                
            Company company = await _context.Companies
                .FirstOrDefaultAsync(a => a.Id == id);
            if (company == null) return NotFound();

            CompanyDataRecordViewModel model = new CompanyDataRecordViewModel
            {
                Address = company.Address,
                Description = company.Description,
                ImagePath = company.ImagePath,
                Name = company.Name,
                PhoneNumber = company.PhoneNumber,
                Type = company.Type,
                Id = company.Id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompanyEditing(CompanyDataRecordViewModel model)
        {
            if (ModelState.IsValid)
            {
                Company company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.Id == model.Id);
                if (company == null) return NotFound();

                // save img
                if (model.PreviewImage != null)
                {
                    string path = "/src/img/companies/" + model.PreviewImage.FileName;
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await model.PreviewImage.CopyToAsync(fileStream);
                    }
                    model.ImagePath = "~" + path;
                }

                string type = string.Empty;
                if (model.Type.Equals("Библиотека"))
                {
                    type = "library";
                }
                else if (model.Type.Equals("Магазин"))
                {
                    type = "store";
                }

                company.Name = model.Name;
                company.PhoneNumber = model.PhoneNumber;
                company.ImagePath = model.ImagePath;
                company.Type = type;
                company.Address = model.Address;
                company.Description = model.Description;
                await _context.SaveChangesAsync();

                return RedirectToAction("CompaniesList", "Companies");
            }
            return RedirectToAction("CompanyEditing", "Companies", new { id = model.Id });
        }


        [HttpGet]
        public async Task<IActionResult> RemovingCompany(int? id)
        {
            if (id == null) return NotFound();

            Company company = await _context.Companies
                .FirstOrDefaultAsync(a => a.Id == id);
            if (company == null) return NotFound();

            bool storageExists = await _context.Storages
                .AnyAsync(s => s.CompanyId == id);

            if (storageExists)
                ViewBag.RecordExists = true;
            else ViewBag.RecordExists = false;

            return View(company);
        }

        [HttpPost]
        public async Task<IActionResult> RemovingCompany(Company company)
        {
            /*
            var storages = await _context.Storages
                .Where(f => f.CompanyId == company.Id)
                .ToListAsync();
            foreach (var storage in storages)
            {
                storage.CompanyId = null;
                await _context.SaveChangesAsync();

                // copies
                var copies = await _context.Copies
                    .Where(c => c.StorageId == storage.Id)
                    .ToListAsync();
                foreach (var copy in copies)
                {
                    copy.StorageId = null;
                    await _context.SaveChangesAsync();
                }

                // employees
                var employees = await _context.Employees
                    .Where(c => c.StorageId == storage.Id)
                    .ToListAsync();
                foreach (var employee in employees)
                {
                    employee.StorageId = null;
                    await _context.SaveChangesAsync();
                }
            }

            _context.Storages.RemoveRange(storages);
            await _context.SaveChangesAsync();
            */
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction("CompaniesList", "Companies");
        }
    }
}
