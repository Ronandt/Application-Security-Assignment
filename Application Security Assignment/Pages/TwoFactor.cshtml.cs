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
        private readonly IEmailSenderService _emailSenderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IFilterSessionService _filterSessionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _logService;
        [BindProperty(SupportsGet =true)]
        public string Email { get; set; }

        [BindProperty]
        public string Token { get; set; }
        public TwoFactorModel(IEmailSenderService emailSenderService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IFilterSessionService filterSessionService, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _emailSenderService = emailSenderService;
            _userManager = userManager;
            _signInManager = signInManager;
            _filterSessionService = filterSessionService;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnGet()
        {

            var user = await _userManager.FindByEmailAsync(Email);
            if(user is not null)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                await _emailSenderService.SendEmailAsync(Email, "Your two factor code", $"{await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider)}");
            }
         


        }
        public async Task<IActionResult> OnPost()
        {
         

            if(await _userManager.VerifyTwoFactorTokenAsync(await _signInManager.GetTwoFactorAuthenticationUserAsync(), TokenOptions.DefaultEmailProvider, Token))
            {
            

                var identityResult = await _signInManager.TwoFactorSignInAsync(TokenOptions.DefaultEmailProvider, Token, true, true);


                if (identityResult.Succeeded)
                {

                    _filterSessionService.StoreUserSession(Email, _httpContextAccessor);
                    await _logService.LogUser(Data.Enums.Actions.Login,  Email);
                    return RedirectToPage("/Index");
                }
                else if (identityResult.IsLockedOut)
                {
                    ModelState.AddModelError("", "You have been locked out");
                    return Page();
                }
                else if(identityResult.RequiresTwoFactor)
                {
                    TempData["error"] = "What the fuck?";
                 }
            };
            TempData["error"] = "Invalid code!";
            return Page();
        }


    }
}
