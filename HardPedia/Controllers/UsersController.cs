using HardPedia.Data;
using HardPedia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HardPedia.Controllers
{
    [Authorize(Roles = "Admin")]
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

        // edit a user
        public IActionResult EditUser(string id)
        {
            IdentityUser user = _context.Users.Find(id);
            if (user == null)
            {
                   return NotFound();
            }
            // get the role
            var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);
            var role = _context.Roles.Find(userRole.RoleId);
            
            if (role == null)
            {
                   return NotFound();
            }

            var userDTO = new UserDTO(user.Id, user.UserName, user.Email, user.PhoneNumber, role.Name);

            // get the role names from db
            var roles = _context.Roles.Select(categorie => new SelectListItem { Text = categorie.Name, Value = categorie.Name }).ToList();
            
            ViewBag.roles = roles;

            return View(userDTO);
        }

        [HttpPost]
        public IActionResult EditUser(UserDTO user)
        {
            var updatedUser = _context.Users.Find(user.Id);
            updatedUser.UserName = user.UserName;
            updatedUser.NormalizedUserName = user.UserName.ToUpper();
            updatedUser.Email = user.Email;
            updatedUser.NormalizedEmail = user.Email.ToUpper();
            updatedUser.PhoneNumber = user.PhoneNumber;

            // get the role id from name
            
            var roleId = _context.Roles.FirstOrDefault(r => r.Name == user.Role).Id;

            
            var oldUserInRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == user.Id);

            var newUserInRole = new IdentityUserRole<string>
            {
                UserId = user.Id,
                RoleId = roleId
            };

            _context.UserRoles.Remove(oldUserInRole);
            _context.SaveChanges();

            _context.UserRoles.Add(newUserInRole);
            _context.SaveChanges();
            
            return RedirectToAction("ListUsers");

        }


    }
}
