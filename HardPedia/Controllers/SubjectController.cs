using Microsoft.AspNetCore.Mvc;
using HardPedia.Data;
using HardPedia.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HardPedia.Controllers
{
    public class SubjectController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult AddSubject(Guid categoryId)
        {
            ViewBag.CategoryId = categoryId;
            return View();
        }

        [HttpPost]

        public IActionResult AddSubject(Guid categoryId, Subject subject)
        {
            if (ModelState.IsValid)
            {
                subject.Id = Guid.NewGuid();
                subject.CreatedOn = DateTime.Now;
                subject.Visible = true; 

                var category = _context.Categories.Include(c => c.Subjects).FirstOrDefault(c => c.Id == categoryId);
                if (category != null)
                {
                    subject.Categories.Add(category);
                    category.Subjects.Add(subject);
                    _context.Subjects.Add(subject);
                    _context.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }
            ViewBag.CategoryId = categoryId;
            return View(subject);
        }
    }
}
