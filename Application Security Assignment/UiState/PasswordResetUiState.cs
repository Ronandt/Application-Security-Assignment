using System.ComponentModel.DataAnnotations;

namespace Application_Security_Assignment.UiState
{
	public class PasswordResetUiState
	{
		[Required]
		public string? Email { get; set; }
	}
}
