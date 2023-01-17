using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
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
        public LogoutModel(SignInManager<ApplicationUser> signInManager, IFilterSessionService filterSessionService, IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _filterSessionService = filterSessionService;
            this.httpContextAccessor = httpContextAccessor;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            if(User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                _filterSessionService.ClearSession(httpContextAccessor);
                

            }
            
            return Redirect("/login");
        }
    }
}
