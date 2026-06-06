using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlanner.Core.Models;
using StudyPlanner.Core.ViewModels;

namespace StudyPlanner_WebApp.Controllers
{
	[Authorize]
	public class ProfileController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public ProfileController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return NotFound();

			var model = new EditProfileViewModel
			{
				FirstName = user.FirstName ?? "",
				LastName = user.LastName ?? "",
				DateOfBirth = user.DateOfBirth,
				Email = user.Email
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Index(EditProfileViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var user = await _userManager.GetUserAsync(User);
			if (user == null) return NotFound();

			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.DateOfBirth = model.DateOfBirth;

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				TempData["Success"] = "Профилът е обновен успешно!";
				return RedirectToAction(nameof(Index));
			}

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return View(model);
		}

		[HttpGet]
		public IActionResult ChangePassword()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid) return View(model);

			var user = await _userManager.GetUserAsync(User);
			if (user == null) return NotFound();

			var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
			if (result.Succeeded)
			{
				await _signInManager.RefreshSignInAsync(user);
				TempData["Success"] = "Паролата е сменена успешно!";
				return RedirectToAction(nameof(Index));
			}

			foreach (var error in result.Errors)
				ModelState.AddModelError(string.Empty, error.Description);

			return View(model);
		}
	}
}