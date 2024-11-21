using Demo.DAL.Models;
using Demo.PL.Models;
using Demo.PL.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace Demo.PL.Controllers

{


	
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
		}
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel) {

			if (!ModelState.IsValid)
			{
				return View(registerViewModel);


			}
			var user = new ApplicationUser
			{
				FirstName = registerViewModel.FirstName,
				LastName = registerViewModel.LastName,
				Email = registerViewModel.Email,
				UserName = registerViewModel.UserName,
			};

			var result = await userManager.CreateAsync(user, registerViewModel.Password);
			// identity result
			if (result.Succeeded)
			{
				return RedirectToAction("Login");
			}
			else
			{
				foreach (var item in result.Errors) {
					ModelState.AddModelError(string.Empty, item.Description);

				}
				return View(registerViewModel);
			}
		}

		public IActionResult Login() {
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(loginViewModel);
			}
			var user = await userManager.FindByEmailAsync(loginViewModel.Email);
			if (user is not null)
			{
				if (await userManager.CheckPasswordAsync(user, loginViewModel.Password))
				{
					var result =await signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
					if (result.Succeeded)
					{
						return RedirectToAction(nameof(HomeController.Index), nameof(HomeController).Replace("Controller", ""));
					}
				}

			}

			ModelState.AddModelError("", "Incorrect Email or Password");
			return View(loginViewModel);
		}

		public new IActionResult SignOut()
		{
			signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}


		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var user = await userManager.FindByEmailAsync(model.Email);
			if(user is not null)
			{
				var token = await userManager.GeneratePasswordResetTokenAsync(user);
				var url = Url.Action(nameof(ResetPassword),nameof(AccountController).Replace("Controller",""),new { Email = model.Email,Token = token}, Request.Scheme);
				var email = new Email
				{
					Subject = "Reset Password",
					Body = url!,
					Recipent = model.Email
				};

				SentEmailSettings.SendEmailAsync(email); 

				return RedirectToAction(nameof(CheckYourInBox));
			}

			ModelState.AddModelError("", "Email is not found in this website");
			return View(model);

		}
		public IActionResult CheckYourInBox()
		{
			return View();
		}

		public IActionResult ResetPassword(string Email,string Token)
		{
			if (Email is null || Token is null) return BadRequest();
			TempData["Email"] = Email;
			TempData["Token"] = Token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
		{
			resetPasswordViewModel.Token= TempData["Token"]?.ToString() ?? string.Empty;
			resetPasswordViewModel.Email = TempData["Email"]?.ToString() ?? string.Empty;

			if (!ModelState.IsValid)
			{
				return View(resetPasswordViewModel);
			}

			var user = await userManager.FindByEmailAsync(resetPasswordViewModel.Email);
			if(user is not null)
			{
				var result = await userManager.ResetPasswordAsync(user,resetPasswordViewModel.Token, resetPasswordViewModel.Password);
				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
                }
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
				}
			}
			
			return View(resetPasswordViewModel);


		}

	} 
}
