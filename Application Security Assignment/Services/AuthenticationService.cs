

using Microsoft.AspNetCore.Identity;
using Application_Security_Assignment.Data.Models;
using Web.Utils;
using System.Web.Helpers;
using NuGet.Common;
using System.Security.Claims;

namespace Application_Security_Assignment.Services
{
 
    public class AuthenticationService
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogService _logService;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IFilterSessionService _filterSessionService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSenderService _emailSenderService;

        public AuthenticationService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogService logService, IResetPasswordService resetPasswordService, IFilterSessionService filterSessionService, IHttpContextAccessor httpContextAccessor, IEmailSenderService emailSenderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logService = logService;
            _resetPasswordService = resetPasswordService;
            _filterSessionService = filterSessionService;
            _httpContextAccessor = httpContextAccessor;
            _emailSenderService = emailSenderService;   
        }

        public async Task<Result<SignInResult>> LocalLogin(ApplicationUser applicationUser, string password)
        {
          
            if (applicationUser != null && await _userManager.CheckPasswordAsync(applicationUser, password))
            {
                await _userManager.UpdateSecurityStampAsync(applicationUser);
            }

            var identityResult = await _signInManager.PasswordSignInAsync(applicationUser.Email, password, true, true);

            if (identityResult.Succeeded)
            {
                await ExecuteBaseLogin(applicationUser);
         
            }
            return Result<SignInResult>.Success("Log in operation success", identityResult);
        }

        public async Task Send2FACode(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is not null)
            {
                await _userManager.UpdateSecurityStampAsync(user);
                await _emailSenderService.SendEmailAsync(Email, "Your two factor code", $"{await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider)}");
            }
        }

        public async Task<Result<Boolean>> TwoFALogin(string Token)
        {
            ApplicationUser user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, Token))
            {


                var identityResult = await _signInManager.TwoFactorSignInAsync(TokenOptions.DefaultEmailProvider, Token, true, true);


                if (identityResult.Succeeded)
                {

                   await ExecuteBaseLogin(user);
                    return Result<Boolean>.Success("Sign in operation success!", true);

                   
                }
                else if (identityResult.IsLockedOut)
                {
                    return Result<Boolean>.Success("You are locked out", false);
                }
                else if (identityResult.RequiresTwoFactor)
                {
                    return Result<Boolean>.Success("Two factor authentication required", false);
                }
            };

            return Result<Boolean>.Success("Invalid code!", false);


    
        }

        public async Task<Result<Boolean>> ExternalLogin()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (await _userManager.FindByNameAsync(info.Principal.FindFirstValue(ClaimTypes.Email)) != null)
            {
                await _userManager.UpdateSecurityStampAsync(await _userManager.FindByNameAsync(info.Principal.FindFirstValue(ClaimTypes.Email)));
            }
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: true, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {

                await ExecuteBaseLogin(info);
                return Result<Boolean>.Success("Login success!", true);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user == null)
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
                    await ExecuteBaseLogin(info);
                }
                return Result<Boolean>.Success("Register success!", true);
            }

            return Result<Boolean>.Success("Something happened with the login!", false);
        }

        private async Task ExecuteBaseLogin(ApplicationUser applicationUser)
        {
            _filterSessionService.StoreUserSession(applicationUser.Email, _httpContextAccessor);
            await _logService.LogUser(Data.Enums.Actions.Login, applicationUser.Email);

        }

        private async Task ExecuteBaseLogin(ExternalLoginInfo applicationUser)
        {
            _filterSessionService.StoreUserSession(applicationUser.Principal.FindFirstValue(ClaimTypes.Email), _httpContextAccessor);
            await _logService.LogUser(Data.Enums.Actions.Login, applicationUser.Principal.FindFirstValue(ClaimTypes.Email));

        }



    }

   
}
