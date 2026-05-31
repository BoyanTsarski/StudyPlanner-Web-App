using StudyPlanner.Core.Models;

namespace StudyPlanner.Core.Interfaces
{
	public interface ITaskService
	{
		Task<IEnumerable<StudyTask>> GetAllTasksAsync(string userId);
		Task<IEnumerable<StudyTask>> GetFilteredTasksAsync(string userId, string search, string priority, string status, int page, int pageSize);
		Task<int> GetTasksCountAsync(string userId, string search, string priority, string status);
		Task<StudyTask> GetTaskByIdAsync(int id);
		Task CreateTaskAsync(StudyTask task);
		Task UpdateTaskAsync(StudyTask task);
		Task DeleteTaskAsync(int id);
		Task AddSubTaskAsync(int taskId, string title);
		Task ToggleSubTaskAsync(int subTaskId);
	}
}