using HardPedia.Data;
using HardPedia.Models;
using HardPedia.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HardPedia.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        
        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        // get comment by id
        public IActionResult GetComment(Guid id)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == id);
            return View(comment);
        }

        // add comment
        [HttpGet]
        public IActionResult AddComment(Guid subjectId)
        {
            // get the user id
            var userName = "";
            var userId = "";
            try
            {
                userName = User.Identity.Name;
                userId = _context.Users.FirstOrDefault(u => u.UserName == userName).Id;
            }
            catch
            {
                return RedirectToAction("Login", "Account");
            }
            

            ViewBag.SubjectId = subjectId;
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        public IActionResult AddComment(Guid subjectId, string userId, Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Id = Guid.NewGuid();
                comment.SubjectId = subjectId;
                comment.UserId = userId;
                comment.CreatedOn = DateTime.Now;
                _context.Comments.Add(comment);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            ViewBag.SubjectId = subjectId;
            ViewBag.UserId = userId;
            return View(comment);
        }

    }
}
