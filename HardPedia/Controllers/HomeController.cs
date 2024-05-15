using HardPedia.Data;
using HardPedia.Models;
using HardPedia.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HardPedia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            var categories = await _context.Categories
                .Include(c => c.Subjects)
                .ToListAsync();

            var categoryViewModels = categories.Select(c => new CategorySubjectsViewModel(
                c.Id,
                c.Name,
                c.Description,
                c.Subjects.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(),
                (int)Math.Ceiling(c.Subjects.Count / (double)pageSize),
                pageNumber
            )).ToList();

            return View(categoryViewModels);
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
