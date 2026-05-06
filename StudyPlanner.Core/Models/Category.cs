using System.ComponentModel.DataAnnotations;

namespace StudyPlanner.Core.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(50, ErrorMessage = "Името не може да е повече от 50 символа")]
        public string Name { get; set; }

        [StringLength(7, ErrorMessage = "Невалиден цвят")]
        public string ColorHex { get; set; }

        public ICollection<StudyTask> Tasks { get; set; }
    }
}