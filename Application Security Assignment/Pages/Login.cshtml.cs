using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity;
using Application_Security_Assignment.Data.Models;
using static System.Net.WebRequestMethods;
using Application_Security_Assignment.Services;

namespace Application_Security_Assignment.Pages
{


    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFilterSessionService _filterSessionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogService _logService;
     
        public LoginModel(SignInManager<ApplicationUser> signInManager, IFilterSessionService filterSessionService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ILogService logService)
        {
            _signInManager = signInManager;
            _filterSessionService = filterSessionService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _logService = logService;
        }

        [BindProperty]

        public LoginUiState LoginUiState { get; set; } = new LoginUiState();
        public async Task<IActionResult> OnGet()
        {
            LoginUiState.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
      
            if (!_filterSessionService.CheckUserSession(_httpContextAccessor).Value && HttpContext.User.Identity.IsAuthenticated)
            {
                await _logService.LogUser(Data.Enums.Actions.Logout, (await _userManager.GetUserAsync(User)).Email);
                await _signInManager.SignOutAsync();
                return Redirect("/login");
            }
       
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoginUiState.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {

                var identityResult = await _signInManager.PasswordSignInAsync(LoginUiState.Email, LoginUiState.Password, true, true);

                if (identityResult.Succeeded)
                {
              
                    _filterSessionService.StoreUserSession(LoginUiState.Email, _httpContextAccessor);
                    await _logService.LogUser(Data.Enums.Actions.Login, LoginUiState.Email);
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

        public async Task<IActionResult> OnPostGoogleAsync()
        {
       
            return new ChallengeResult("Google", _signInManager.ConfigureExternalAuthenticationProperties("Google", "/ExternalLogin"));

        }
 
    }

    }



