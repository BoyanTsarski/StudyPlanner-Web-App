using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using StudyPlanner.Core.Enums;
using TaskStatus = System.Threading.Tasks.TaskStatus;

namespace StudyPlanner.Core.Models
{
    public class StudyTask
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заглавието е задължително")]
        [StringLength(100, ErrorMessage = "Заглавието не може да е повече от 100 символа")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Описанието не може да е повече от 500 символа")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Крайният срок е задължителен")]
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