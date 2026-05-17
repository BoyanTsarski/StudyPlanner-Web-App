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
}