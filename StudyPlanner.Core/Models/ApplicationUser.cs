using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;
using StudyPlanner.Core.Models;

namespace StudyPlanner.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public ICollection<StudyTask> Tasks { get; set; }
        public ICollection<Exam> Exams { get; set; }
    }
}