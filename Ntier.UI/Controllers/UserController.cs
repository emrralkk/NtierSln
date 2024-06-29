using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ntier.Data.Models.Identity;

namespace Ntier.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var admins = await _userManager.GetUsersInRoleAsync("Admin");
            var users=new List<AppUser>();
            foreach (var item in admins) 
                users=_userManager.Users.Where(x=>x.Id == item.Id).ToList();


            
            return View(users);
        }
    }
}
