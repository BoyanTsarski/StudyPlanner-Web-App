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
    }
}