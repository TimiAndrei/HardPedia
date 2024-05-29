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
        // Check if user is authenticated
        if (!User.Identity.IsAuthenticated)
        {
            return Redirect("~/Identity/Account/Login?ReturnUrl=" + Url.Action("AddSubject", "Subject", new { categoryId }));
        }

        // User is authenticated, get the user id
        var userName = User.Identity.Name;
        var userId = _context.Users.FirstOrDefault(u => u.UserName == userName)?.Id;

        // User is authenticated but id is not found
        if (userId == null)
        {
            return RedirectToAction("Error", "Home");
        }

        ViewBag.CategoryId = categoryId;
        ViewBag.UserId = userId;
        return View();
    }

    [HttpPost]
    public IActionResult AddSubject(Guid categoryId, string userId, Subject subject)
    {
        if (ModelState.IsValid)
        {
            subject.Id = Guid.NewGuid();
            subject.CreatedOn = DateTime.Now;
            subject.Visible = true;
            subject.UserId = userId; 

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
        ViewBag.UserId = userId;
        return View(subject);
    }

    [HttpGet]
    public IActionResult EditSubject(Guid id)
    {
        var subject = _context.Subjects.FirstOrDefault(s => s.Id == id);
        if (subject == null)
        {
            return NotFound();
        }

        return View(subject);
    }

    [HttpPost]
    public IActionResult EditSubject(Subject editedSubject)
    {
        var subject = _context.Subjects.FirstOrDefault(s => s.Id == editedSubject.Id);
        if (subject == null)
        {
            return NotFound();
        }

        subject.Heading = editedSubject.Heading;
        subject.Title = editedSubject.Title;
        subject.Content = editedSubject.Content;
        _context.SaveChanges();

        return RedirectToAction("ListSubject", "Subject", new { id = subject.Id });
    }

    [HttpPost]
    public IActionResult DeleteSubject(Guid id)
    {
        var subject = _context.Subjects.FirstOrDefault(s => s.Id == id);
        if (subject == null)
        {
            return NotFound();
        }

        _context.Subjects.Remove(subject);
        _context.SaveChanges();

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

    public IActionResult SortComments(Guid id, string sortBy)
    {
        var subject = _context.Subjects
                             .Include(s => s.Categories)
                             .Include(s => s.Comments)
                                 .ThenInclude(c => c.User)
                             .FirstOrDefault(s => s.Id == id);

        if (subject != null)
        {
            switch (sortBy)
            {
                case "newest":
                    subject.Comments = subject.Comments.OrderByDescending(c => c.CreatedOn).ToList();
                    break;
                case "oldest":
                    subject.Comments = subject.Comments.OrderBy(c => c.CreatedOn).ToList();
                    break;
                default:
                    break;
            }

            return View("ListSubject", subject);
        }

        return RedirectToAction("Index", "Home");
    }


}
