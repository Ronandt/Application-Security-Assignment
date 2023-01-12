using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Security_Assignment.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterUiState RegisterUiState { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var newUser = new ApplicationUser()
            {

            };

            var result = await _userManager.CreateAsync(newUser, Password);
            return Page();
        }
    }
}
