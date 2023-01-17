using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity;
using Application_Security_Assignment.Data.Models;
using static System.Net.WebRequestMethods;

namespace Application_Security_Assignment.Pages
{



    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginModel(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public LoginUiState LoginUiState { get; set; }
        public void OnGet()
        {
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var identityResult = await _signInManager.PasswordSignInAsync(LoginUiState.Email, LoginUiState.Password, true, true);

                if (identityResult.Succeeded)
                {
                   
                    return RedirectToPage("/Index");
                }
                ModelState.AddModelError("", "Username or Password incorrect.");
            }
            return Page();
        }
    }
    }
