using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Security_Assignment.Pages
{
    public class PasswordResetModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderService _emailSender;
        public PasswordResetModel(UserManager<ApplicationUser> userManager, IEmailSenderService emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }
        [BindProperty]
        public PasswordResetUiState PasswordResetUiState { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await _emailSender.SendEmailAsync(PasswordResetUiState.Email, "Password reset token",await _userManager.GeneratePasswordResetTokenAsync(await _userManager.FindByEmailAsync(PasswordResetUiState.Email)));

                TempData["success"] = "Email sent!";
         


   
            }
            return Page();
        }
    }
}
