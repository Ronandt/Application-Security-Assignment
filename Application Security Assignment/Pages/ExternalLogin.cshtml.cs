using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using System.Runtime;
using System.Security.Claims;

namespace Application_Security_Assignment.Pages
{
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFilterSessionService _filterSessionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _logService;
        public ExternalLoginModel(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IFilterSessionService filterSessionService, IHttpContextAccessor httpContextAccessor, ILogService logService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _filterSessionService = filterSessionService;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }
        public async Task<IActionResult> OnGet()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if(await _userManager.FindByNameAsync(info.Principal.FindFirstValue(ClaimTypes.Email)) != null)
            {
                await _userManager.UpdateSecurityStampAsync(await _userManager.FindByNameAsync(info.Principal.FindFirstValue(ClaimTypes.Email)));
            }
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if(signInResult.Succeeded)
            {
                TempData["success"] = "Login success!";
                _filterSessionService.StoreUserSession(info.Principal.FindFirstValue(ClaimTypes.Email), _httpContextAccessor);
                _logService.LogUser(Data.Enums.Actions.Login, info.Principal.FindFirstValue(ClaimTypes.Email));
                return Redirect("/");
            } else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if(email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if(user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),

                        };
                        await _userManager.CreateAsync(user);
                    }
                    await _userManager.AddLoginAsync(user, info); //adds exteranl login info that links the email together 
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    _filterSessionService.StoreUserSession(info.Principal.FindFirstValue(ClaimTypes.Email), _httpContextAccessor);
                    _logService.LogUser(Data.Enums.Actions.Login, info.Principal.FindFirstValue(ClaimTypes.Email));
                }
                TempData["success"] = "Register success!";
                return Redirect("/");
            }

            return Redirect("/login");
        }
    }
}
