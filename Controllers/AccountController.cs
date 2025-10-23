using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UNI_ASSETS.Models;
using UNI_ASSETS.Models.ViewModels;

namespace UNI_ASSETS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<AppUser> signInManager;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    await signInManager.SignInAsync(user, model.IsPersistent);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Update(AppUser user)
        {
            if (ModelState.IsValid)
            {
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Users", "Account");
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Users()
        {
            var users = _userManager.Users;
            return View(users);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
           await _userManager.DeleteAsync(user);
            return RedirectToAction("Users");
        }
        [HttpGet]
        public IActionResult Add()
        {
            PopulateDDL();
            return View();
            
        }
        void PopulateDDL(object selected = null)
        {

            ViewBag.Roles = new SelectList(roleManager.Roles,"RoleId","Role",selected);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser {UserName=model.Username,Email=model.Email,PhoneNumber=model.PhoneNumber };
               
              var result =await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                   await _userManager.AddToRoleAsync(user, model.Role);
                }
                return RedirectToAction("Users");
            }
            PopulateDDL(model.Role);
            return View(model);
        }
    }
}
