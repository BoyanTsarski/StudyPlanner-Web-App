using System.ComponentModel.DataAnnotations;

namespace StudyPlanner.Core.Models
{
	public class Exam
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Предметът е задължителен")]
		[StringLength(100, ErrorMessage = "Предметът не може да е повече от 100 символа")]
		public string Subject { get; set; }

		[Required(ErrorMessage = "Датата е задължителна")]
		public DateTime ExamDate { get; set; }

		[StringLength(100, ErrorMessage = "Залата не може да е повече от 100 символа")]
		public string? Location { get; set; }

		[StringLength(500, ErrorMessage = "Бележките не могат да са повече от 500 символа")]
		public string? Notes { get; set; }

		public string? UserId { get; set; }
		public ApplicationUser? User { get; set; }
	}
}