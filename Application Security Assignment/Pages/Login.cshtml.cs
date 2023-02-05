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
        private readonly AuthenticationService _authenticationService;
        private readonly PrepopulationService _prepopulationService;
     
        public LoginModel(SignInManager<ApplicationUser> signInManager, IFilterSessionService filterSessionService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, ILogService logService, AuthenticationService authentication, PrepopulationService prepopulationService)
        {
            _signInManager = signInManager;
            _filterSessionService = filterSessionService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _logService = logService;
            _authenticationService = authentication;
            _prepopulationService = prepopulationService;
        }

        [BindProperty]

        public LoginUiState LoginUiState { get; set; } = new LoginUiState();
        public async Task<IActionResult> OnGet()
        {
            LoginUiState.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            await _prepopulationService.PrepopulateAdmin();
            if (!_filterSessionService.CheckUserSession(_httpContextAccessor).Value && HttpContext.User.Identity.IsAuthenticated)
            {
                await _logService.LogUser(Data.Enums.Actions.Logout, (await _userManager.GetUserAsync(User)).Email);
                TempData["error"] = "Your session has timed out!";
                await _signInManager.SignOutAsync();
                return Redirect("/login");
            }
            if (!HttpContext.User.Identity.IsAuthenticated && _filterSessionService.CheckUserSession(_httpContextAccessor).Value)
            {
                _filterSessionService.ClearSession(_httpContextAccessor);
            }
       
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoginUiState.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
             
                var user = await _userManager.FindByNameAsync(LoginUiState.Email);
           


                var identityResult = (await _authenticationService.LocalLogin(user, LoginUiState.Password)).Value;

                if (identityResult.Succeeded)
                {
              
                    return RedirectToPage("/Index");
                }
                else if(identityResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "You have been locked out");
                    return Page();
                }
                else if(identityResult.RequiresTwoFactor)
                {
                    
                    return Redirect($"/twofactor?email={LoginUiState.Email}");
                }
            
            }
            TempData["error"] = "Incorrect credentials!";
            return Page();
        }

        public async Task<IActionResult> OnPostGoogleAsync()
        {
       
            return new ChallengeResult("Google", _signInManager.ConfigureExternalAuthenticationProperties("Google", "/ExternalLogin"));

        }
 
    }

    }



