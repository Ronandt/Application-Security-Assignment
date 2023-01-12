using Application_Security_Assignment.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application_Security_Assignment.UiState
{
    public class RegisterUiState
    {
        [Required]
        public string? FullName { get; set; }

        public string? CreditCardNo { get; set; }

        [Required]
        public Gender? Gender { get; set; }

        [Required]
        public string? MobileNo { get; set; }
        [Required]
        public string? DeliveryAddress { get; set; }

        public string? ImageURL { get; set; }

        public string? AboutMe { get; set; }

        public string? ConfirmPassword { get; set; }

        public string? Password { get; set; }

        public string? Email { get; set; }
    }
}
