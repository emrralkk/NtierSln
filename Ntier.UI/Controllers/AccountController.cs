using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ntier.Data.Models.Identity;
using Ntier.Data.Models.ViewModels;

namespace Ntier.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            if (User.Identity.Name != null)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
                user = await _userManager.FindByEmailAsync(login.UserName);

            if(user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe, true);
                if(result.Succeeded)
                    return RedirectToAction("Index", "Home");
            }
            return View(login); 
        }



        public IActionResult Register()
        {
            if (User.Identity.Name != null)
                return RedirectToAction("Index");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            var user = new AppUser()
            {
                Name = register.Name,
                Surname = register.Surname,
                UserName = register.Username,
                Email = register.Email,
            };

            var result = await _userManager.CreateAsync(user, register.Password);

            //Rolü oluştur veya veya varsa al
            var roleExixts = await _roleManager.RoleExistsAsync("User");

            AppRole role;

            if (!roleExixts)
            {
                //Role oluştur
                role = new AppRole("User");
                await _roleManager.CreateAsync(role);
            }
            else
                //Role al
                role = await _roleManager.FindByNameAsync("User");
            

            //Kullanıcıya rolü ata
            await _userManager.AddToRoleAsync(user, role.Name);
            if(result.Succeeded)
                return RedirectToAction("Login", "Account");

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description); 
            }

            return View(register);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
