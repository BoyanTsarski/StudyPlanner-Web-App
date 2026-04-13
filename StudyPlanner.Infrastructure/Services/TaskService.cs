using Microsoft.EntityFrameworkCore;
using StudyPlanner.Core.Interfaces;
using StudyPlanner.Core.Models;

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
    }
}