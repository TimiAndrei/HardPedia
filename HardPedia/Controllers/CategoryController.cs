using Microsoft.AspNetCore.Mvc;
using HardPedia.Data;
using HardPedia.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HardPedia.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var categories = _context.Categories.Include(c => c.Subjects).ToList();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Search(string query)
    {
        // If query is empty, return all categories
        if (string.IsNullOrEmpty(query))
        {
            return PartialView("_CategoryListPartial", (List<Category>?)_context.Categories.Include(c => c.Subjects).ToList());
        }

        // Search for categories
        var categories = _context.Categories
            .Where(c => c.Name.Contains(query) || c.Description.Contains(query))
            .Include(c => c.Subjects)
            .ToList();

        if (!categories.Any())
        {
            
            // Search for subjects
            var subjects = _context.Subjects
                .Where(s => s.Title.Contains(query) || s.Content.Contains(query))
                .Include(s => s.Categories)
                .ToList();

            if (!subjects.Any())
            {
                return NotFound();
            }

            categories = subjects.SelectMany(s => s.Categories).Distinct().ToList();
        }

        return PartialView("_CategoryListPartial", categories);
    }

    [HttpGet]
    public IActionResult AddCategory()
    {
        return View();
    }


    [HttpPost]
    public IActionResult AddCategory(Category category)
    {
        if (ModelState.IsValid)
        {
            category.Id = Guid.NewGuid();
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult EditCategory(Guid id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    public IActionResult UpdateCategory(Guid id, Category category)
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(c => c.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index", "Home");
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult DeleteCategory(Guid id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult GetSubject(Guid categoryId, Guid currentSubjectId, string direction)
    {
        var category = _context.Categories
            .Include(c => c.Subjects)
            .FirstOrDefault(c => c.Id == categoryId);

        if (category == null)
        {
            return NotFound();
        }

        var subjects = category.Subjects.OrderByDescending(s => s.CreatedOn).ToList();
        var currentIndex = subjects.FindIndex(s => s.Id == currentSubjectId);

        int newIndex;
        if (direction == "next")
        {
            newIndex = (currentIndex - 1 + subjects.Count) % subjects.Count; 
        }
        else if (direction == "previous")
        {
            newIndex = (currentIndex + 1) % subjects.Count;
        }
        else
        {
            return BadRequest("Invalid direction");
        }

        var newSubject = subjects[newIndex];
        return Json(new
        {
            id = newSubject.Id,
            title = newSubject.Title,
            shortContent = newSubject.GetShortContent()
        });
    }

}
