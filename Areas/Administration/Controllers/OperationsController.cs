using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Administration.Models.Operations;
using WebApp.Models.Database;

namespace WebApp.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "admin")]
    public class OperationsController : Controller
    {
        private readonly DatabaseContext _context;

        public OperationsController(DatabaseContext context)
        {
            _context = context;
        }

        private IQueryable<OperationRecordViewModel> GetOperationsList(string type)
        {
            var clients = _context.Clients
                .Join(_context.Operations, c => c.Id, o => o.ClientId, (c, o) => new
                {
                    c.UniqueKey,
                    o.Id
                });

            var employees = _context.Employees
                .Include(e => e.Storage)
                .ThenInclude(s => s.Company)
                .Join(_context.Operations, e => e.Id, o => o.EmployeeId, (e, o) => new
                {
                    e.Storage.Company.Type,
                    e.UniqueKey,
                    o.Id
                });

            var copies = _context.Copies
                .Join(_context.Operations, c => c.Id, o => o.CopyId, (c, o) => new
                {
                    c.UniqueKey,
                    o.Type,
                    o.Status,
                    o.CreationDate,
                    o.Id
                });

            var operations = clients
                .Join(employees.Where(e => e.Type.Equals(type)), c => c.Id, e => e.Id, (c, e) => new
                {
                    ClientUniqueKey = c.UniqueKey,
                    EmployeeUniqueKey = e.UniqueKey,
                    OperationId = c.Id
                })
                .Join(copies, e => e.OperationId, c => c.Id, (e, c) => new OperationRecordViewModel
                {
                    ClientUniqueKey = e.ClientUniqueKey,
                    EmployeeUniqueKey = e.EmployeeUniqueKey,
                    CopyUniqueKey = c.UniqueKey,
                    Date = c.CreationDate,
                    Type = c.Type,
                    Id = c.Id,
                    Status = c.Status
                }).Distinct();

            return operations;
        }

        public async Task<IActionResult> LibraryOperationsList(string search)
        {
            ViewBag.SearchData = search;

            var operations = GetOperationsList("library");

            if (!string.IsNullOrEmpty(search))
                operations = operations
                    .Where(o => o.Type.Contains(search) || o.Status.Contains(search));

            return View(await operations.ToListAsync());
        }

        public async Task<IActionResult> StoreOperationsList(string search)
        {
            ViewBag.SearchData = search;

            var operations = GetOperationsList("store");

            if (!string.IsNullOrEmpty(search))
                operations = operations
                    .Where(o => o.Type.Contains(search) || o.Status.Contains(search));

            return View(await operations.ToListAsync());
        }
    }
}
