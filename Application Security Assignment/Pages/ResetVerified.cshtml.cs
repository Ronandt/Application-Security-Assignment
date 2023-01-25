using Application_Security_Assignment.Data.Models;
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
        
        public string Password { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Email { get; set; }

        [BindProperty(SupportsGet = true), Required]
        public string Token { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public ResetVerifiedModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
 
            if (ModelState.IsValid)
            {
                if (!(await _userManager.ResetPasswordAsync(await _userManager.FindByEmailAsync(Email), Token, Password)).Succeeded)
                {
                    TempData["error"] = "Invalid token";
                    return Page();
                }
                TempData["success"] = "Password has been reset!";
                return Redirect("/login");

            }
            return Page();

        }
    }
}
