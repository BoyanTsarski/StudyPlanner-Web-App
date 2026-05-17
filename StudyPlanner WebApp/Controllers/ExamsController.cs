using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;

namespace StudyPlanner_WebApp.Controllers
{
    [Authorize]
    public class ExamsController : Controller
    {
        private readonly IExamService _examService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ExamsController(IExamService examService, UserManager<ApplicationUser> userManager)
        {
            _examService = examService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string search, int page = 1)
        {
            int pageSize = 10;
            var userId = _userManager.GetUserId(User);

            var exams = await _examService.GetFilteredExamsAsync(userId, search, page, pageSize);
            var totalCount = await _examService.GetExamsCountAsync(userId, search);

            ViewBag.Search = search;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return View(exams);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                exam.UserId = _userManager.GetUserId(User);
                await _examService.CreateExamAsync(exam);
                TempData["Success"] = "Изпитът е създаден успешно!";
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null) return NotFound();
            return View(exam);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Exam exam)
        {
            if (ModelState.IsValid)
            {
                await _examService.UpdateExamAsync(exam);
                TempData["Success"] = "Изпитът е обновен успешно!";
                return RedirectToAction(nameof(Index));
            }
            return View(exam);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _examService.DeleteExamAsync(id);
            TempData["Success"] = "Изпитът е изтрит успешно!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id)
        {
            var exam = await _examService.GetExamByIdAsync(id);
            if (exam == null) return NotFound();
            return View(exam);
        }
    }
}