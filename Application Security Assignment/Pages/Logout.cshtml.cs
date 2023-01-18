using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Security_Assignment.Pages
{
    [Authorize]
 
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IFilterSessionService _filterSessionService;
        private readonly ILogService _logService;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public LogoutModel(SignInManager<ApplicationUser> signInManager, IFilterSessionService filterSessionService, IHttpContextAccessor httpContextAccessor, ILogService logService, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _filterSessionService = filterSessionService;
            this.httpContextAccessor = httpContextAccessor;
            _logService = logService;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
               await _logService.LogUser(Data.Enums.Actions.Logout, (await _userManager.GetUserAsync(User)).Email);
             await _signInManager.SignOutAsync();

                _filterSessionService.ClearSession(httpContextAccessor);
             

            }

            return Redirect("/login");
        }

        
    }
}
