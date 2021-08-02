using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using WebApp.Models;
using WebApp.Models.Database;
using System.Threading.Tasks;
using WebApp.Models.Home;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var books = _context.Publications
                .Join(_context.Publishers, b => b.PublisherId, p => p.Id, (b, p) => new BookDataViewModel
                {
                    PublisherName = p.Name,
                    BookId = b.Id,
                    BookName = b.Name,
                    Description = b.Description,
                    PublicationId = b.Id,
                    PreviewImage = b.ImagePath,
                    Type = b.Type,
                    Year = b.Year
                });

            var result = _context.Copies
                .Where(c => c.Status.Equals("available") && c.Count > 0)
                .OrderByDescending(c => c.ModifiedDate)
                .Join(books, c => c.PublicationId, b => b.PublicationId, (c, b) => b)
                .Distinct()
                .Take(5);

            return View(await result.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
