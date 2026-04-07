using StudyPlanner.Core.Models;

namespace StudyPlanner.Core.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime ExamDate { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
