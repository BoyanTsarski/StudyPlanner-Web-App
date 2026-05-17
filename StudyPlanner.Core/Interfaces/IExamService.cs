using StudyPlanner.Core.Models;

namespace StudyPlanner.Core.Interfaces
{
    public interface IExamService
    {
        Task<IEnumerable<Exam>> GetAllExamsAsync(string userId);
        Task<IEnumerable<Exam>> GetFilteredExamsAsync(string userId, string search, int page, int pageSize);
        Task<int> GetExamsCountAsync(string userId, string search);
        Task<Exam> GetExamByIdAsync(int id);
        Task CreateExamAsync(Exam exam);
        Task UpdateExamAsync(Exam exam);
        Task DeleteExamAsync(int id);
    }
}