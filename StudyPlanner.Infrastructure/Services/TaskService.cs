using Microsoft.EntityFrameworkCore;
using StudyPlanner.Core.Enums;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;
using TaskStatus = StudyPlanner.Core.Enums.TaskStatus;

namespace StudyPlanner.Infrastructure.Services
{
	public class TaskService : ITaskService
	{
		private readonly ApplicationDbContext _context;

		public TaskService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<StudyTask>> GetAllTasksAsync(string userId)
		{
			return await _context.Tasks
				.Where(t => t.UserId == userId && !t.IsDeleted)
				.Include(t => t.Category)
				.Include(t => t.SubTasks)
				.OrderBy(t => t.DueDate)
				.ToListAsync();
		}

		public async Task<IEnumerable<StudyTask>> GetFilteredTasksAsync(string userId, string search, string priority, string status, int page, int pageSize)
		{
			var query = _context.Tasks
				.Where(t => t.UserId == userId && !t.IsDeleted)
				.Include(t => t.Category)
				.Include(t => t.SubTasks)
				.AsQueryable();

			if (!string.IsNullOrEmpty(search))
				query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));

			if (!string.IsNullOrEmpty(priority) && Enum.TryParse<Priority>(priority, out var p))
				query = query.Where(t => t.Priority == p);

			if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskStatus>(status, out var s))
				query = query.Where(t => t.Status == s);

			return await query
				.OrderBy(t => t.DueDate)
				.Skip((page - 1) * pageSize)
				.Take(pageSize)
				.ToListAsync();
		}

		public async Task<int> GetTasksCountAsync(string userId, string search, string priority, string status)
		{
			var query = _context.Tasks
				.Where(t => t.UserId == userId && !t.IsDeleted)
				.AsQueryable();

			if (!string.IsNullOrEmpty(search))
				query = query.Where(t => t.Title.Contains(search) || t.Description.Contains(search));

			if (!string.IsNullOrEmpty(priority) && Enum.TryParse<Priority>(priority, out var p))
				query = query.Where(t => t.Priority == p);

			if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskStatus>(status, out var s))
				query = query.Where(t => t.Status == s);

			return await query.CountAsync();
		}

		public async Task<StudyTask> GetTaskByIdAsync(int id)
		{
			return await _context.Tasks
				.Include(t => t.Category)
				.Include(t => t.SubTasks)
				.FirstOrDefaultAsync(t => t.Id == id);
		}

		public async Task CreateTaskAsync(StudyTask task)
		{
			_context.Tasks.Add(task);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateTaskAsync(StudyTask task)
		{
			_context.Tasks.Update(task);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteTaskAsync(int id)
		{
			var task = await _context.Tasks.FindAsync(id);
			if (task != null)
			{
				task.IsDeleted = true;
				await _context.SaveChangesAsync();
			}
		}

		public async Task AddSubTaskAsync(int taskId, string title)
		{
			var subTask = new SubTask
			{
				Title = title,
				IsCompleted = false,
				StudyTaskId = taskId
			};
			_context.SubTasks.Add(subTask);
			await _context.SaveChangesAsync();
		}

		public async Task ToggleSubTaskAsync(int subTaskId)
		{
			var subTask = await _context.SubTasks.FindAsync(subTaskId);
			if (subTask != null)
			{
				subTask.IsCompleted = !subTask.IsCompleted;
				await _context.SaveChangesAsync();
			}
		}
	}
}