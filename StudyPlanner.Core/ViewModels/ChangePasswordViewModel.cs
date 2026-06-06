using System.ComponentModel.DataAnnotations;

namespace StudyPlanner.Core.ViewModels
{
	public class ChangePasswordViewModel
	{
		[Required(ErrorMessage = "Полето е задължително")]
		[DataType(DataType.Password)]
		[Display(Name = "Текуща парола")]
		public string CurrentPassword { get; set; } = null!;

		[Required(ErrorMessage = "Полето е задължително")]
		[MinLength(6, ErrorMessage = "Минимум 6 символа")]
		[DataType(DataType.Password)]
		[Display(Name = "Нова парола")]
		public string NewPassword { get; set; } = null!;

		[DataType(DataType.Password)]
		[Display(Name = "Потвърди нова парола")]
		[Compare("NewPassword", ErrorMessage = "Паролите не съвпадат")]
		public string ConfirmPassword { get; set; } = null!;
	}
}