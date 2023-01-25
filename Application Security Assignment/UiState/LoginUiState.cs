using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace Application_Security_Assignment.UiState
{
    public class LoginUiState
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string? Password { get; set; }

        public string? ReturnUrl { get; set; }

        public  IList<AuthenticationScheme>? ExternalLogins { get; set; }


    }
}
