using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;

namespace StudyPlanner_WebApp.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IExamService _examService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CalendarController(ITaskService taskService, IExamService examService, UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _examService = examService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var userId = _userManager.GetUserId(User);
            var tasks = await _taskService.GetAllTasksAsync(userId);
            var exams = await _examService.GetAllExamsAsync(userId);

            var events = new List<object>();

            foreach (var task in tasks)
            {
                events.Add(new
                {
                    id = "task_" + task.Id,
                    title = task.Title,
                    start = task.DueDate.ToString("yyyy-MM-dd"),
                    color = task.Priority == StudyPlanner.Core.Enums.Priority.High ? "#E24B4A" :
                            task.Priority == StudyPlanner.Core.Enums.Priority.Medium ? "#EF9F27" : "#1D9E75",
                    url = "/Tasks/Details/" + task.Id
                });
            }

            foreach (var exam in exams)
            {
                events.Add(new
                {
                    id = "exam_" + exam.Id,
                    title = "Изпит: " + exam.Subject,
                    start = exam.ExamDate.ToString("yyyy-MM-ddTHH:mm"),
                    color = "#378ADD",
                    url = "/Exams/Details/" + exam.Id
                });
            }

            return Json(events);
        }
    }
}