using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity;
using Application_Security_Assignment.Data.Models;
using static System.Net.WebRequestMethods;
using Application_Security_Assignment.Services;

namespace Application_Security_Assignment.Pages
{


   
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFilterSessionService _filterSessionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
     
        public LoginModel(SignInManager<ApplicationUser> signInManager, IFilterSessionService filterSessionService, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _filterSessionService = filterSessionService;
            _httpContextAccessor = httpContextAccessor;
        }

        [BindProperty]
 
        public LoginUiState LoginUiState { get; set; }
        public IActionResult OnGet()
        {
            if (!_filterSessionService.CheckUserSession(_httpContextAccessor).Value && HttpContext.User.Identity.IsAuthenticated)
            {
                _signInManager.SignOutAsync();
                return Redirect("/login");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {

                var identityResult = await _signInManager.PasswordSignInAsync(LoginUiState.Email, LoginUiState.Password, true, true);

                if (identityResult.Succeeded)
                {

                    _filterSessionService.StoreUserSession(LoginUiState.Email, _httpContextAccessor);
                    return RedirectToPage("/Index");
                }
                if(identityResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "You have been locked out");
                    return Page();
                }
                ModelState.AddModelError("", "Username or Password incorrect.");
            }
            return Page();
        }
    }
    }
