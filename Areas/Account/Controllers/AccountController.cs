using AspMvcAuth.Areas.Account.Models;
using AspMvcAuth.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

namespace AspMvcAuth.Areas.Account.Controllers
{
    [Area("Account")]
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly EmailHelper _emailHelper;


        public AccountController(UserManager<ApplicationUser> manager, SignInManager<ApplicationUser> signInManager, EmailHelper emailHelper)
		{
			_userManager = manager;
			_signInManager = signInManager;
			_emailHelper = emailHelper;
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
            ApplicationUser user = await _userManager.FindByNameAsync(model.Login);
            bool emailStatus = await _userManager.IsEmailConfirmedAsync(user);
            if (emailStatus == false)
            {
                ModelState.AddModelError(nameof(user.Email), "Email не подтвержден. Пожалуйста, пройдите по ссылке из отправленного вам письма");
                return View("Login");
            }

            var result = await _signInManager.PasswordSignInAsync(model.Login, model.Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
			{
                
				   return LocalRedirect("/");
			}
			else
			{
				ModelState.AddModelError("", "Неверный логин или пароль");
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
				var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, Request.Scheme);
				//EmailHelper emailHelper = new();
				bool emailResponse = _emailHelper.SendEmail(user.Email, confirmationLink);

				if (emailResponse)
				{
					user = await _userManager.FindByEmailAsync(user.Email);
					//добавляем дату рождения в Claimd пользователя
					await _userManager.AddClaimAsync(user, new Claim("Birthday", model.Birthday.ToString("dd.MM.yyyy"), ClaimValueTypes.String));
					return RedirectToAction("Index", "Home");
				}

				else
				{
					ModelState.AddModelError("", "На сервере произошла ошибка. Попробуйте выполнить регистрацию позднее");
					return View();
				}
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

        [Route("{area}/{action}")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
				return View("Error");

			var result = await _userManager.ConfirmEmailAsync(user, token);
			return View(result.Succeeded ? "ConfirmEmail" : "Error");
		}
	}
}
