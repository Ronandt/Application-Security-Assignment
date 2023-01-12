using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;


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
            if(ModelState.IsValid)
            {
                if(!(new Regex("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\\s).{12}").IsMatch(RegisterUiState.Password)))
                {
                    ModelState.AddModelError("RegisterUiState.Password", "Your password needs to be strong!");
                    return Page();
                }

                var newUser = new ApplicationUser()
                {
                    Email = RegisterUiState.Email,
                    FullName = RegisterUiState.FullName,
                    CreditCardNo = RegisterUiState.CreditCardNo,
                    Gender = RegisterUiState.gender,
                    MobileNo = RegisterUiState.MobileNo,
                    DeliveryAddress = RegisterUiState.DeliveryAddress,
                    //imageurl later
                    UserName = RegisterUiState.FullName,
                    AboutMe = RegisterUiState.AboutMe,
                };
                var result = await _userManager.CreateAsync(newUser, RegisterUiState.Password);
                if(result.Succeeded)
                {
                    return Redirect("/login");
                } else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return  Page();
                }
              
            }



            return Page();
        }
    }
}
