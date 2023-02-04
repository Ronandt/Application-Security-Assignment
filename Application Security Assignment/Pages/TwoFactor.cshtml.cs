using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Security_Assignment.Pages
{
    public class TwoFactorModel : PageModel
    {

        private readonly AuthenticationService _authenticationService;
        [BindProperty(SupportsGet =true)]
        public string Email { get; set; }

        [BindProperty]
        public string Token { get; set; }
        public TwoFactorModel( AuthenticationService authenticationService)
        {

            _authenticationService = authenticationService;
        }

        public async Task OnGet()
        {

        await _authenticationService.Send2FACode(Email);
         


        }
        public async Task<IActionResult> OnPost()
        {

            var result = (await _authenticationService.TwoFALogin(Token));

            if(result.Value)
            {
                return RedirectToPage("/Index");
            } else
            {
                TempData["error"] = result.Message;
                return Page();
            }
         

        }


    }
}
