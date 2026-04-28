using StudyPlanner.Core.Enums;
using TaskStatus = StudyPlanner.Core.Enums.TaskStatus;

namespace StudyPlanner.Core.Models
{
    public class StudyTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }      
        public TaskStatus Status { get; set; }       
        public bool IsDeleted { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<SubTask> SubTasks { get; set; }
    }
}
