using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UNI_ASSETS.Data;
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
        private readonly IRepositoryWrapper repository;
        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IRepositoryWrapper repository)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.repository = repository;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
                return View(model);
            }

            var result = await signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            else if (result.IsLockedOut)
            {
                TempData["ErrorMessage"] = "Your account has been locked due to multiple failed login attempts. Try again later.";
            }
            else if (result.IsNotAllowed)
            {
                TempData["ErrorMessage"] = "You are not allowed to log in yet. Please verify your account or contact the administrator.";
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid username or password.";
            }

            return View(model);
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        
        public async Task<IActionResult> Logout()
        {
           await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var role = await _userManager.GetRolesAsync(user);
            if (role.Any())
            {
                var Roleid = await roleManager.FindByNameAsync(role.First());
                PopulateDDL(Roleid.Id);
            }
            else
            {
                PopulateDDL();
            }
            return View(user);
        }
        //[HttpPost]
        //public async Task<IActionResult> Update(AppUser user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await _userManager.UpdateAsync(user);
        //        return RedirectToAction("Users", "Account");
        //    }
        //    var role = await _userManager.GetRolesAsync(user);
        //    PopulateDDL(role.First());
        //    return View(user);
        //}
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(AppUser user, string Role)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id);
                if (existingUser != null)
                {
                    existingUser.UserName = user.UserName;
                    existingUser.Email = user.Email;
                    existingUser.PhoneNumber = user.PhoneNumber;

                    await _userManager.UpdateAsync(existingUser);

                    var userRoles = await _userManager.GetRolesAsync(existingUser);
                    await _userManager.RemoveFromRolesAsync(existingUser, userRoles);
                    var roleName = await roleManager.FindByIdAsync(Role);
                    await _userManager.AddToRoleAsync(existingUser, roleName.Name);
                    repository.StaffRepository.Update(existingUser);
                    repository.Save();
                }
                return RedirectToAction("Index", "Account");
            }

            var role = await _userManager.GetRolesAsync(user);
            PopulateDDL(role.FirstOrDefault());
            return View(user);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            List<KeyValuePair<AppUser, string>> UserRoles = new List<KeyValuePair<AppUser, string>>();
            foreach (var user in users)
            {
                string Role =string.Join(",", await _userManager.GetRolesAsync(user));
                UserRoles.Add(new KeyValuePair<AppUser, string>(user,Role));
                
            }
            return View(UserRoles);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
           await _userManager.DeleteAsync(user);
            repository.StaffRepository.Delete(user);
            repository.Save();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            PopulateDDL();
           // ViewBag.Roles = roleManager.Roles.Select(x => x.Name).ToList();
            return View();
            
        }
        void PopulateDDL(object selected = null)
        {

            ViewBag.Roles = new SelectList(roleManager.Roles.ToList(),"Id","Name",selected);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser {UserName=model.Username,Email=model.Email,PhoneNumber=model.PhoneNumber };
               
              var result =await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    var Role = await roleManager.FindByIdAsync(model.Role);
                   await _userManager.AddToRoleAsync(user,Role.Name);
                    repository.StaffRepository.Create(user);
                    repository.Save();
                }
                return RedirectToAction("Index");
            }
            PopulateDDL(model.Role);
            return View(model);
        }
    }
}
