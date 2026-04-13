using StudyPlanner.Core.Models;

namespace StudyPlanner.Core.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<StudyTask>> GetAllTasksAsync(string userId);
        Task<StudyTask> GetTaskByIdAsync(int id);
        Task CreateTaskAsync(StudyTask task);
        Task UpdateTaskAsync(StudyTask task);
        Task DeleteTaskAsync(int id);
    }
}