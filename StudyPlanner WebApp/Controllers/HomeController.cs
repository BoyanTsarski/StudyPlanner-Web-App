using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;

namespace StudyPlanner_WebApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ITaskService _taskService;
		private readonly IExamService _examService;
		private readonly UserManager<ApplicationUser> _userManager;

		public HomeController(ILogger<HomeController> logger, ITaskService taskService, IExamService examService, UserManager<ApplicationUser> userManager)
		{
			_logger = logger;
			_taskService = taskService;
			_examService = examService;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			if (!User.Identity!.IsAuthenticated)
				return View();

			var user = await _userManager.GetUserAsync(User);
			var userId = user!.Id;

			var tasks = await _taskService.GetAllTasksAsync(userId);
			var exams = await _examService.GetAllExamsAsync(userId);

			ViewBag.UserName = user.FirstName ?? user.Email;
			ViewBag.ActiveTasks = tasks.Count(t => t.Status != StudyPlanner.Core.Enums.TaskStatus.Done);
			ViewBag.CompletedTasks = tasks.Count(t => t.Status == StudyPlanner.Core.Enums.TaskStatus.Done);
			ViewBag.HighPriority = tasks.Count(t => t.Priority == StudyPlanner.Core.Enums.Priority.High);
			ViewBag.RecentTasks = tasks.OrderByDescending(t => t.DueDate).Take(5).ToList(); 
			ViewBag.UpcomingExams = exams.OrderBy(e => e.ExamDate).Take(3).ToList();

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[Route("Home/Error/{statusCode?}")]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(int statusCode = 0)
		{
			if (statusCode != 0)
				Response.StatusCode = statusCode;
			return View("Error");
		}
	}
}