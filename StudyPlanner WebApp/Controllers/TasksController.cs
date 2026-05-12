using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;

namespace StudyPlanner_WebApp.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TasksController(ITaskService taskService, ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _categoryService = categoryService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string search, string priority, string status, int page = 1)
        {
            int pageSize = 10;
            var userId = _userManager.GetUserId(User);

            var tasks = await _taskService.GetFilteredTasksAsync(userId, search, priority, status, page, pageSize);
            var totalCount = await _taskService.GetTasksCountAsync(userId, search, priority, status);

            ViewBag.Search = search;
            ViewBag.Priority = priority;
            ViewBag.Status = status;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(tasks);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudyTask task)
        {
            if (ModelState.IsValid)
            {
                task.UserId = _userManager.GetUserId(User);
                await _taskService.CreateTaskAsync(task);
                TempData["Success"] = "Задачата е създадена успешно!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(task);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudyTask task)
        {
            if (ModelState.IsValid)
            {
                await _taskService.UpdateTaskAsync(task);
                TempData["Success"] = "Задачата е обновена успешно!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return View(task);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            TempData["Success"] = "Задачата е изтрита успешно!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }
    }
}