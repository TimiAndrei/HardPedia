using Microsoft.AspNetCore.Mvc;
using HardPedia.Data;
using HardPedia.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace HardPedia.Controllers;

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

    [HttpGet]
    public IActionResult EditSubject(Guid id)
    {
        var subject = _context.Subjects.FirstOrDefault(s => s.Id == id);
        if (subject != null)
        {
            return View(subject);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult EditSubject(Subject subject)
    {
        if (ModelState.IsValid)
        {
            var subjectToUpdate = _context.Subjects.FirstOrDefault(s => s.Id == subject.Id);
            if (subjectToUpdate != null)
            {
                subjectToUpdate.Heading = subject.Heading;
                subjectToUpdate.Title = subject.Title;
                subjectToUpdate.Content = subject.Content;
                subjectToUpdate.Visible = subject.Visible;

                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
        return View(subject);
    }

    [HttpGet]
    public IActionResult DeleteSubject(Guid id)
    {
        var subject = _context.Subjects.FirstOrDefault(s => s.Id == id);
        if (subject != null)
        {
            return View(subject);
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult DeleteSubject(Subject subject)
    {
        var subjectToDelete = _context.Subjects.FirstOrDefault(s => s.Id == subject.Id);
        if (subjectToDelete != null)
        {
            _context.Subjects.Remove(subjectToDelete);
            _context.SaveChanges();
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ListSubject(Guid id)
    {
        var subject = _context.Subjects
                             .Include(s => s.Categories)
                             .Include(s => s.Comments)
                                 .ThenInclude(c => c.User)
                             .FirstOrDefault(s => s.Id == id);

        if (subject != null)
        {
            var userName = User.Identity.Name;

            ViewData["UserName"] = userName;
            return View(subject);
        }
        return RedirectToAction("Index", "Home");
    }


}
