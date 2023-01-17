using Application_Security_Assignment.Data.Enums;
using Application_Security_Assignment.Services;
using System.ComponentModel.DataAnnotations;

namespace Application_Security_Assignment.UiState
{
    public class RegisterUiState
    {
        public Gender gender;
        [Required]
        [Display(Name ="Full Name")]
   
        public string? FullName { get; set; }

        [Display(Name = "Credit Card Number")]
        [DataType(DataType.CreditCard)]
        public string? CreditCardNo { get; set; }

        [Required]
        public String? Gender {
            get => gender.ToString();
            set
            {
                gender = (Gender)Enum.Parse(typeof(Gender), value);
            }
        }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? MobileNo { get; set; }
        [Required]
        public string? DeliveryAddress { get; set; }

        
        public IFormFile? Image { get; set; }


        [DataType(DataType.MultilineText)]
        [Display(Name = "About me")]
        public string? AboutMe { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Required]
        [Compare(nameof(Password), ErrorMessage = "Confirm password does not match the password")]
        public string? ConfirmPassword { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(RegexConstants.PASSWORD_PATTERN, ErrorMessage = @"Passwords must be at least 12 characters, least one non alphanumeric character, least one lowercase ('a'-'z')
and at least one uppercase ('A'-'Z')")]
        [Required]
        public string? Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string? Email { get; set; }

     
    }
}
