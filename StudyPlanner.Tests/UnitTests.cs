using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using StudyPlanner.Core.Models;
using StudyPlanner.Core.Enums;
using StudyPlanner.Infrastructure;
using StudyPlanner.Infrastructure.Services;
using TaskStatus = StudyPlanner.Core.Enums.TaskStatus;

namespace StudyPlanner.Tests
{
	public class TaskServiceTests
	{
		private ApplicationDbContext _context;
		private TaskService _taskService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			_context = new ApplicationDbContext(options);
			_taskService = new TaskService(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

		[Test]
		public async Task CreateTask_ShouldAddTaskToDatabase()
		{
			var task = new StudyTask
			{
				Title = "Тест задача",
				DueDate = DateTime.Now.AddDays(7),
				Priority = Priority.High,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			await _taskService.CreateTaskAsync(task);
			var tasks = await _taskService.GetAllTasksAsync("user1");
			Assert.That(tasks.Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task GetTaskById_ShouldReturnCorrectTask()
		{
			var task = new StudyTask
			{
				Title = "Тест задача 2",
				DueDate = DateTime.Now.AddDays(3),
				Priority = Priority.Medium,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			await _taskService.CreateTaskAsync(task);
			var result = await _taskService.GetTaskByIdAsync(task.Id);
			Assert.That(result.Title, Is.EqualTo("Тест задача 2"));
		}

		[Test]
		public async Task DeleteTask_ShouldMarkTaskAsDeleted()
		{
			var task = new StudyTask
			{
				Title = "Задача за изтриване",
				DueDate = DateTime.Now.AddDays(1),
				Priority = Priority.Low,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			await _taskService.CreateTaskAsync(task);
			await _taskService.DeleteTaskAsync(task.Id);
			var tasks = await _taskService.GetAllTasksAsync("user1");
			Assert.That(tasks.Count(), Is.EqualTo(0));
		}

		[Test]
		public async Task UpdateTask_ShouldUpdateTaskInDatabase()
		{
			var task = new StudyTask
			{
				Title = "Оригинална задача",
				DueDate = DateTime.Now.AddDays(5),
				Priority = Priority.Low,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			await _taskService.CreateTaskAsync(task);
			task.Title = "Обновена задача";
			await _taskService.UpdateTaskAsync(task);
			var result = await _taskService.GetTaskByIdAsync(task.Id);
			Assert.That(result.Title, Is.EqualTo("Обновена задача"));
		}

		[Test]
		public async Task GetAllTasks_ShouldReturnOnlyUserTasks()
		{
			var task1 = new StudyTask
			{
				Title = "Задача на user1",
				DueDate = DateTime.Now.AddDays(1),
				Priority = Priority.High,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			var task2 = new StudyTask
			{
				Title = "Задача на user2",
				DueDate = DateTime.Now.AddDays(2),
				Priority = Priority.Low,
				Status = TaskStatus.ToDo,
				UserId = "user2"
			};
			await _taskService.CreateTaskAsync(task1);
			await _taskService.CreateTaskAsync(task2);
			var tasks = await _taskService.GetAllTasksAsync("user1");
			Assert.That(tasks.Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task GetFilteredTasks_ShouldReturnFilteredByPriority()
		{
			var task1 = new StudyTask
			{
				Title = "Висок приоритет",
				DueDate = DateTime.Now.AddDays(1),
				Priority = Priority.High,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			var task2 = new StudyTask
			{
				Title = "Нисък приоритет",
				DueDate = DateTime.Now.AddDays(2),
				Priority = Priority.Low,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			await _taskService.CreateTaskAsync(task1);
			await _taskService.CreateTaskAsync(task2);
			var tasks = await _taskService.GetFilteredTasksAsync("user1", "", "High", "", 1, 10);
			Assert.That(tasks.Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task GetFilteredTasks_ShouldReturnFilteredBySearch()
		{
			var task1 = new StudyTask
			{
				Title = "Математика задача",
				DueDate = DateTime.Now.AddDays(1),
				Priority = Priority.High,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			var task2 = new StudyTask
			{
				Title = "Физика задача",
				DueDate = DateTime.Now.AddDays(2),
				Priority = Priority.Low,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			await _taskService.CreateTaskAsync(task1);
			await _taskService.CreateTaskAsync(task2);
			var tasks = await _taskService.GetFilteredTasksAsync("user1", "Математика", "", "", 1, 10);
			Assert.That(tasks.Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task GetTasksCount_ShouldReturnCorrectCount()
		{
			var task1 = new StudyTask
			{
				Title = "Задача 1",
				DueDate = DateTime.Now.AddDays(1),
				Priority = Priority.High,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			var task2 = new StudyTask
			{
				Title = "Задача 2",
				DueDate = DateTime.Now.AddDays(2),
				Priority = Priority.Low,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			await _taskService.CreateTaskAsync(task1);
			await _taskService.CreateTaskAsync(task2);
			var count = await _taskService.GetTasksCountAsync("user1", "", "", "");
			Assert.That(count, Is.EqualTo(2));
		}

		[Test]
		public async Task CreateTask_ShouldSetCorrectPriority()
		{
			var task = new StudyTask
			{
				Title = "Висок приоритет задача",
				DueDate = DateTime.Now.AddDays(1),
				Priority = Priority.High,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			await _taskService.CreateTaskAsync(task);
			var result = await _taskService.GetTaskByIdAsync(task.Id);
			Assert.That(result.Priority, Is.EqualTo(Priority.High));
		}

		[Test]
		public async Task GetFilteredTasks_ShouldSupportPaging()
		{
			for (int i = 1; i <= 15; i++)
			{
				await _taskService.CreateTaskAsync(new StudyTask
				{
					Title = $"Задача {i}",
					DueDate = DateTime.Now.AddDays(i),
					Priority = Priority.Low,
					Status = TaskStatus.ToDo,
					UserId = "user1"
				});
			}
			var page1 = await _taskService.GetFilteredTasksAsync("user1", "", "", "", 1, 10);
			var page2 = await _taskService.GetFilteredTasksAsync("user1", "", "", "", 2, 10);
			Assert.That(page1.Count(), Is.EqualTo(10));
			Assert.That(page2.Count(), Is.EqualTo(5));
		}

		[Test]
		public async Task GetFilteredTasks_ShouldReturnFilteredByStatus()
		{
			var task1 = new StudyTask
			{
				Title = "Завършена задача",
				DueDate = DateTime.Now.AddDays(1),
				Priority = Priority.Low,
				Status = TaskStatus.Done,
				UserId = "user1"
			};
			var task2 = new StudyTask
			{
				Title = "Незавършена задача",
				DueDate = DateTime.Now.AddDays(2),
				Priority = Priority.Low,
				Status = TaskStatus.ToDo,
				UserId = "user1"
			};
			await _taskService.CreateTaskAsync(task1);
			await _taskService.CreateTaskAsync(task2);
			var tasks = await _taskService.GetFilteredTasksAsync("user1", "", "", "Done", 1, 10);
			Assert.That(tasks.Count(), Is.EqualTo(1));
		}
	}

	public class CategoryServiceTests
	{
		private ApplicationDbContext _context;
		private CategoryService _categoryService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			_context = new ApplicationDbContext(options);
			_categoryService = new CategoryService(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

		[Test]
		public async Task CreateCategory_ShouldAddCategoryToDatabase()
		{
			var category = new Category { Name = "Математика", ColorHex = "#378ADD" };
			await _categoryService.CreateCategoryAsync(category);
			var categories = await _categoryService.GetAllCategoriesAsync();
			Assert.That(categories.Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task GetCategoryById_ShouldReturnCorrectCategory()
		{
			var category = new Category { Name = "Физика", ColorHex = "#D85A30" };
			await _categoryService.CreateCategoryAsync(category);
			var result = await _categoryService.GetCategoryByIdAsync(category.Id);
			Assert.That(result.Name, Is.EqualTo("Физика"));
		}

		[Test]
		public async Task DeleteCategory_ShouldRemoveCategoryFromDatabase()
		{
			var category = new Category { Name = "За изтриване", ColorHex = "#888" };
			await _categoryService.CreateCategoryAsync(category);
			await _categoryService.DeleteCategoryAsync(category.Id);
			var categories = await _categoryService.GetAllCategoriesAsync();
			Assert.That(categories.Count(), Is.EqualTo(0));
		}

		[Test]
		public async Task UpdateCategory_ShouldUpdateNameInDatabase()
		{
			var category = new Category { Name = "Стара", ColorHex = "#888" };
			await _categoryService.CreateCategoryAsync(category);
			category.Name = "Нова";
			await _categoryService.UpdateCategoryAsync(category);
			var result = await _categoryService.GetCategoryByIdAsync(category.Id);
			Assert.That(result.Name, Is.EqualTo("Нова"));
		}

		[Test]
		public async Task GetAllCategories_ShouldReturnAllCategories()
		{
			await _categoryService.CreateCategoryAsync(new Category { Name = "Категория 1", ColorHex = "#111" });
			await _categoryService.CreateCategoryAsync(new Category { Name = "Категория 2", ColorHex = "#222" });
			await _categoryService.CreateCategoryAsync(new Category { Name = "Категория 3", ColorHex = "#333" });
			var categories = await _categoryService.GetAllCategoriesAsync();
			Assert.That(categories.Count(), Is.EqualTo(3));
		}

		[Test]
		public async Task DeleteCategory_ShouldSetTasksCategoryToNull()
		{
			var category = new Category { Name = "Тест", ColorHex = "#888" };
			await _categoryService.CreateCategoryAsync(category);
			var task = new StudyTask
			{
				Title = "Задача с категория",
				DueDate = DateTime.Now.AddDays(1),
				Priority = Priority.Low,
				Status = TaskStatus.ToDo,
				UserId = "user1",
				CategoryId = category.Id
			};
			_context.Tasks.Add(task);
			await _context.SaveChangesAsync();
			await _categoryService.DeleteCategoryAsync(category.Id);
			var updatedTask = await _context.Tasks.FindAsync(task.Id);
			Assert.That(updatedTask!.CategoryId, Is.Null);
		}
	}

	public class ExamServiceTests
	{
		private ApplicationDbContext _context;
		private ExamService _examService;

		[SetUp]
		public void Setup()
		{
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;
			_context = new ApplicationDbContext(options);
			_examService = new ExamService(_context);
		}

		[TearDown]
		public void TearDown()
		{
			_context.Dispose();
		}

		[Test]
		public async Task CreateExam_ShouldAddExamToDatabase()
		{
			var exam = new Exam
			{
				Subject = "Математика",
				ExamDate = DateTime.Now.AddDays(7),
				Location = "Зала 1",
				UserId = "user1"
			};
			await _examService.CreateExamAsync(exam);
			var exams = await _examService.GetAllExamsAsync("user1");
			Assert.That(exams.Count(), Is.EqualTo(1));
		}

		[Test]
		public async Task DeleteExam_ShouldRemoveExamFromDatabase()
		{
			var exam = new Exam
			{
				Subject = "Физика",
				ExamDate = DateTime.Now.AddDays(3),
				Location = "Зала 2",
				UserId = "user1"
			};
			await _examService.CreateExamAsync(exam);
			await _examService.DeleteExamAsync(exam.Id);
			var exams = await _examService.GetAllExamsAsync("user1");
			Assert.That(exams.Count(), Is.EqualTo(0));
		}

		[Test]
		public async Task GetFilteredExams_ShouldReturnFilteredBySearch()
		{
			await _examService.CreateExamAsync(new Exam
			{
				Subject = "Математика",
				ExamDate = DateTime.Now.AddDays(1),
				Location = "Зала 1",
				UserId = "user1"
			});
			await _examService.CreateExamAsync(new Exam
			{
				Subject = "История",
				ExamDate = DateTime.Now.AddDays(2),
				Location = "Зала 3",
				UserId = "user1"
			});
			var exams = await _examService.GetFilteredExamsAsync("user1", "Математика", 1, 10);
			Assert.That(exams.Count(), Is.EqualTo(1));
		}
	}
}