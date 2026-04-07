
namespace StudyPlanner.Core.Models
{
    public class SubTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }

        public int StudyTaskId { get; set; }
        public StudyTask StudyTask { get; set; }
    }

}
