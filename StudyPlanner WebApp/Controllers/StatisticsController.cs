using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;

namespace StudyPlanner_WebApp.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IExamService _examService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsController(ITaskService taskService, IExamService examService, UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _examService = examService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var tasks = await _taskService.GetAllTasksAsync(userId);
            var exams = await _examService.GetAllExamsAsync(userId);

            ViewBag.TotalTasks = tasks.Count();
            ViewBag.CompletedTasks = tasks.Count(t => t.Status == StudyPlanner.Core.Enums.TaskStatus.Done);
            ViewBag.PendingTasks = tasks.Count(t => t.Status == StudyPlanner.Core.Enums.TaskStatus.ToDo);
            ViewBag.InProgressTasks = tasks.Count(t => t.Status == StudyPlanner.Core.Enums.TaskStatus.InProgress);
            ViewBag.TotalExams = exams.Count();
            ViewBag.HighPriority = tasks.Count(t => t.Priority == StudyPlanner.Core.Enums.Priority.High);
            ViewBag.MediumPriority = tasks.Count(t => t.Priority == StudyPlanner.Core.Enums.Priority.Medium);
            ViewBag.LowPriority = tasks.Count(t => t.Priority == StudyPlanner.Core.Enums.Priority.Low);

            return View();
        }
    }
}