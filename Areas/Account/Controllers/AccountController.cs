using AspMvcAuth.Areas.Account.Models;
using AspMvcAuth.Data;
using AspMvcAuth.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspMvcAuth.Areas.Account.Controllers
{
    [Area("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        
        public AccountController(UserManager<ApplicationUser> manager, SignInManager<ApplicationUser> signInManager) 
        {
            _userManager = manager;
            _signInManager = signInManager;
            
        }

		[Route("{area}/{action}")]
		public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("/");
        }

        [Route("{area}/{action}")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginModel model)
        {
           var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, lockoutOnFailure: false);
           if (result.Succeeded)
            {
                return LocalRedirect("/");
            }
           else
             {
                ModelState.AddModelError("", $"Задан неверный логин или пароль");
             }
			return View("Login");
		}

        [Route("{area}/{action}")]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View("Login");
        }


        [Route("{area}/{action}")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Route("{area}/{action}")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            var user = model.GetUser();
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            { 
                foreach (var error in result.Errors) 
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
        }
    }
}
