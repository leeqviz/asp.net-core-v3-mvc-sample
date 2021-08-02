using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Catalog.Models.Service;
using WebApp.Models;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Catalog.Controllers
{
    [Area("Catalog")]
    public class ServiceController : Controller
    {
        private readonly DatabaseContext _context;

        public ServiceController(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> AllCompanies(string search, string sort, string type, int page = 1)
        {
            var companies = _context.Companies
                .Select(c => c);

            if (!string.IsNullOrEmpty(search))
                companies = companies
                    .Where(c => c.Name.Contains(search) || c.Address.Contains(search));

            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.Equals("Название"))
                    companies = companies.OrderByDescending(c => c.Name);
                else if (sort.Equals("Адрес"))
                    companies = companies.OrderByDescending(c => c.Address);
            }

            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("Библиотека"))
                    companies = companies.Where(c => c.Type.Equals("library"));
                else if (type.Equals("Магазин"))
                    companies = companies.Where(c => c.Type.Equals("store"));
            }

            int pageSize = 6;
            var count = await companies.CountAsync();
            var items = await companies
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ServiceViewModel model = new ServiceViewModel
            {
                Companies = items,
                PageViewModel = new PageViewModel(count, page, pageSize),
                Search = search,
                Sort = sort,
                Type = type
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> CompanyDetail(int? id)
        {
            if (id == null) return NotFound();
            Company company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == id);
            if (company == null) return NotFound();

            var storages = await _context.Storages
                .Where(s => s.CompanyId == company.Id).ToListAsync();

            CompanyDetailViewModel model = new CompanyDetailViewModel
            {
                Company = company,
                Storages = storages
            };

            return View(model);
        }
    }
}
