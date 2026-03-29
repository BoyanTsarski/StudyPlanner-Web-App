using Microsoft.AspNetCore.Identity;

namespace StudyPlanner_WebApp.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }

        public ICollection<StudyTask> Tasks { get; set; }
        public ICollection<Exam> Exams { get; set; }
    }
}
