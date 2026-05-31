using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;

namespace StudyPlanner_WebApp.Controllers
{
	[Authorize]
	public class CategoriesController : Controller
	{
		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{
			_categoryService = categoryService;
		}

		public async Task<IActionResult> Index()
		{
			var categories = await _categoryService.GetAllCategoriesAsync();
			return View(categories);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Category category)
		{
			if (ModelState.IsValid)
			{
				await _categoryService.CreateCategoryAsync(category);
				TempData["Success"] = "Категорията е създадена успешно!";
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var category = await _categoryService.GetCategoryByIdAsync(id);
			if (category == null) return NotFound();
			return View(category);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Category category)
		{
			if (ModelState.IsValid)
			{
				await _categoryService.UpdateCategoryAsync(category);
				TempData["Success"] = "Категорията е обновена успешно!";
				return RedirectToAction(nameof(Index));
			}
			return View(category);
		}

		public async Task<IActionResult> Delete(int id)
		{
			await _categoryService.DeleteCategoryAsync(id);
			TempData["Success"] = "Категорията е изтрита успешно!";
			return RedirectToAction(nameof(Index));
		}
	}
}