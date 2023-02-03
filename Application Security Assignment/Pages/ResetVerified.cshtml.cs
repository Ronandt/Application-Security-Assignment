using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Application_Security_Assignment.Pages
{
    public class ResetVerifiedModel : PageModel
    {
        [BindProperty]
        [DataType(DataType.Password)]
        [RegularExpression(RegexConstants.PASSWORD_PATTERN, ErrorMessage = @"Passwords must be at least 12 characters, least one non alphanumeric character, least one lowercase ('a'-'z')
and at least one uppercase ('A'-'Z')")]
        [Required]
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public string Token { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResetPasswordService _resetPasswordService;
        public ResetVerifiedModel(UserManager<ApplicationUser> userManager, IResetPasswordService resetPasswordService)
        {
            _userManager = userManager;
            _resetPasswordService = resetPasswordService;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
 
            if (ModelState.IsValid)
            {
                var resetPasswordResult = await _resetPasswordService.ResetPassword(Email, Token, Password);
                if (!(resetPasswordResult.Value))
                {
                    TempData["error"] = resetPasswordResult.Message;
                    return Page();
                }
                TempData["success"] = resetPasswordResult.Message;
                return Redirect("/login");

            }
            return Page();

        }
    }
}
