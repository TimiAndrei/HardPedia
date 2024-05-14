using HardPedia.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HardPedia.Controllers
{
    public class UsersController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public UsersController(ILogger<HomeController> logger, ApplicationDbContext appContext)
        {
            _logger = logger;
            _context = appContext;
        }

        

        public IActionResult ListUsers()
        {   
            List<IdentityUser> users = _context.Users.ToList();
            return View(users);
        }


    }
}
