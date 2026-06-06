using System.ComponentModel.DataAnnotations;

namespace StudyPlanner.Core.ViewModels
{
	public class EditProfileViewModel
	{
		[Required(ErrorMessage = "Името е задължително")]
		[Display(Name = "Име")]
		public string FirstName { get; set; } = null!;

		[Required(ErrorMessage = "Фамилията е задължителна")]
		[Display(Name = "Фамилия")]
		public string LastName { get; set; } = null!;

		[Display(Name = "Дата на раждане")]
		public DateTime? DateOfBirth { get; set; }

		[Display(Name = "Имейл")]
		public string? Email { get; set; }
	}
}