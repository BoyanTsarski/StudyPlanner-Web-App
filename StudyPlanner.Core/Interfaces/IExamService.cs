using StudyPlanner.Core.Models;

namespace StudyPlanner.Core.Interfaces
{
    public interface IExamService
    {
        Task<IEnumerable<Exam>> GetAllExamsAsync(string userId);
        Task<Exam> GetExamByIdAsync(int id);
        Task CreateExamAsync(Exam exam);
        Task UpdateExamAsync(Exam exam);
        Task DeleteExamAsync(int id);
    }
}