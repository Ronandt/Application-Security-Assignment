using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Application_Security_Assignment.Pages
{
    [Authorize]
    public class ChangePasswordModel : PageModel
    {


        private readonly IResetPasswordService _resetPasswordService;
        private readonly UserManager<ApplicationUser> _userManager;
        [BindProperty]
        public string CurrentPassword { get; set; }

        [BindProperty]
        public string NewPassword { get; set; }
        public ChangePasswordModel(IResetPasswordService resetPasswordService, UserManager<ApplicationUser> userManager)
        {
            _resetPasswordService = resetPasswordService;
            _userManager = userManager;
        }
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(ModelState.IsValid)
            {

                var result = await _resetPasswordService.ResetPassword(await _userManager.GetUserAsync(User), CurrentPassword, NewPassword);
                if(!result.Value)
                {
                    TempData["error"] = result.Message;
                    return Page();
                }
                TempData["success"] = result.Message;
                return Redirect("/");

            }
         
            return Page();
        }
    }
}
