using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Management.Models.OperationsViewer;
using WebApp.Models.Database;
using WebApp.Models.Database.Entities;

namespace WebApp.Areas.Management.Controllers
{
    [Area("Management")]
    [Authorize(Roles = "employee")]
    public class OrdersController : Controller
    {
        private readonly DatabaseContext _context;

        public OrdersController(DatabaseContext context)
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

        public async Task<IActionResult> HandledOperations(string search)
        {
            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();
            if (employee.StorageId != null)
            {
                var operations = (from cl in _context.Clients
                                  join o in _context.Operations
                                  on cl.Id equals o.ClientId
                                  join cp in _context.Copies
                                  on o.CopyId equals cp.Id
                                  where !o.Status.Equals("processing") && cp.StorageId == employee.StorageId
                                  select new OperationViewModel
                                  {
                                      ClientUniqueKey = cl.UniqueKey,
                                      CopyUniqueKey = cp.UniqueKey,
                                      Date = o.CreationDate,
                                      Type = o.Type,
                                      Status = o.Status,
                                      Id = o.Id
                                  });

                ViewBag.SearchData = search;

                Storage storage = await _context.Storages
                    .FirstOrDefaultAsync(s => s.Id == employee.StorageId);
                if (storage == null) return NotFound();

                Company company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.Id == storage.CompanyId);
                if (company == null) return NotFound();

                ViewBag.CompanyType = company.Type;

                if (company.Type.Equals("store"))
                {
                    operations = operations
                        .Where(o => o.Type.Equals("purchase"))
                        .Join(_context.Orders, op => op.Id, or => or.OperationId, (op, or) => new OperationViewModel
                        {
                            ClientUniqueKey = op.ClientUniqueKey,
                            CopyUniqueKey = op.CopyUniqueKey,
                            Date = op.Date,
                            Type = op.Type,
                            Status = op.Status,
                            Id = op.Id,
                            Order = or
                        });
                }
                else
                {
                    operations = operations
                        .Where(o => !o.Type.Equals("purchase"));
                }

                if (!string.IsNullOrEmpty(search))
                    operations = operations
                        .Where(o => o.ClientUniqueKey.Contains(search) || o.CopyUniqueKey.Contains(search));

                return View(await operations.Distinct().ToListAsync());
            }
            else return NotFound();
        }

        public async Task<IActionResult> UnhandledOperations(string search)
        {
            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();
            if (employee.StorageId != null)
            {
                var operations = (from cl in _context.Clients
                                  join o in _context.Operations
                                  on cl.Id equals o.ClientId
                                  join cp in _context.Copies
                                  on o.CopyId equals cp.Id
                                  where o.Status.Equals("processing") && cp.StorageId == employee.StorageId
                                  select new OperationViewModel
                                  {
                                      ClientUniqueKey = cl.UniqueKey,
                                      CopyUniqueKey = cp.UniqueKey,
                                      Date = o.CreationDate,
                                      Type = o.Type,
                                      Id = o.Id
                                  });

                ViewBag.SearchData = search;

                Storage storage = await _context.Storages
                    .FirstOrDefaultAsync(s => s.Id == employee.StorageId);
                if (storage == null) return NotFound();

                Company company = await _context.Companies
                    .FirstOrDefaultAsync(c => c.Id == storage.CompanyId);
                if (company == null) return NotFound();

                ViewBag.CompanyType = company.Type;

                if (company.Type.Equals("store"))
                {
                    operations = operations
                        .Where(o => o.Type.Equals("purchase"))
                        .Join(_context.Orders, op => op.Id, or => or.OperationId, (op, or) => new OperationViewModel
                        {
                            ClientUniqueKey = op.ClientUniqueKey,
                            CopyUniqueKey = op.CopyUniqueKey,
                            Date = op.Date,
                            Type = op.Type,
                            Status = op.Status,
                            Id = op.Id,
                            Order = or
                        });
                }
                else
                {
                    operations = operations
                        .Where(o => !o.Type.Equals("purchase"));
                }

                if (!string.IsNullOrEmpty(search))
                    operations = operations
                        .Where(o => o.ClientUniqueKey.Contains(search) || o.CopyUniqueKey.Contains(search));

                return View(await operations.Distinct().ToListAsync());
            }
            else return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> AcceptOperation(int? id)
        {
            if (id == null) return NotFound();

            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();
            if (employee.StorageId != null)
            {
                Operation operation = await _context.Operations
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (operation == null) return NotFound();

                Copy copy = await _context.Copies
                    .FirstOrDefaultAsync(c => c.Id == operation.CopyId);

                if (copy.Count >= 1)
                {
                    if (operation.Type.Equals("receive") || operation.Type.Equals("purchase")) copy.Count -= 1;
                    if (operation.Type.Equals("return")) copy.Count += 1;
                    await _context.SaveChangesAsync();

                    operation.EmployeeId = employee.Id;
                    operation.Status = "accepted";
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("UnhandledOperations", "Orders");
            }
            else return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> DeclineOperation(int? id)
        {
            if (id == null) return NotFound();

            Employee employee = await GetEmployee();
            if (employee == null) return NotFound();
            if (employee.StorageId != null)
            {
                Operation operation = await _context.Operations
                    .FirstOrDefaultAsync(o => o.Id == id);
                if (operation == null) return NotFound();

                operation.EmployeeId = employee.Id;
                operation.Status = "declined";
                await _context.SaveChangesAsync();

                return RedirectToAction("UnhandledOperations", "Orders");
            }
            else return NotFound();
        }
    }
}
