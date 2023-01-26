using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Web.Utils;
using Application_Security_Assignment.Data.Models;
using System.Web;
using NuGet.Common;
using System.Web.Helpers;
using Application_Security_Assignment.Data.Database.WebApp_Core_Identity.Model;
using System.Linq;

namespace Application_Security_Assignment.Services
{
    public class ResetPasswordService: IResetPasswordService
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        private const string RESET_LINK = "resetverified";
        private readonly AuthDbContext _authDbContext;
        public const long MAX_PASSWORD_LENGTH_SECONDS = 864000 * 3; //10 days * 3 (30days)
        public const long MIN_PASSWORD_LENGTH_SECONDS = 100; //one and a half minutes
        public ResetPasswordService(IEmailSenderService emailSenderService, UserManager<ApplicationUser> userManager, AuthDbContext authDbContext)
        {
            _emailSenderService = emailSenderService;
            _userManager = userManager;
            _authDbContext = authDbContext;
          ;
        }
        public async Task SendPasswordResetEmail(string email)
        {
            await _emailSenderService.SendEmailAsync(email, "Password reset token", $"https://localhost:7042/{RESET_LINK}?Token={HttpUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(await _userManager.FindByEmailAsync(email)))}&Email={email}");
        }
        public async Task<Result<bool>> ResetPassword(string email, string token, string password)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            string oldPassword = user.PasswordHash;
            var Presult = VerifyPasswordWithPasswordPolicy(user, password);
            if (!Presult.Value)
            {
                return Result<bool>.Success(Presult.Message, false);
            }
            else if(!(await _userManager.ResetPasswordAsync(user, token, password)).Succeeded)
            {
               
                return Result<bool>.Success("Invalid token", false);
            }

            user.PasswordCreation = DateTimeOffset.Now.ToUnixTimeSeconds();
            user.PreviousPasswordHash = oldPassword;
            await _userManager.UpdateAsync(user);
            return Result<bool>.Success("Password resetted!", true);
        }

        public async Task<Result<bool>> ResetPassword(ApplicationUser user, string currentPassword, string newPassword)
        {
            string oldPassword = user.PasswordHash;
            var Presult = VerifyPasswordWithPasswordPolicy(user, newPassword);
            if (Presult.Value)
            {
          
                var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
                if (result.Succeeded)
                {
                    user.PasswordCreation = DateTimeOffset.Now.ToUnixTimeSeconds();
                    user.PreviousPasswordHash = oldPassword;
                    await _userManager.UpdateAsync(user);
                    return Result<bool>.Success(Presult.Message, true);

                } 
                return Result<bool>.Success(String.Join(", ", result.Errors.Select( x => x.Description)), false);
               
            } 
           return Result<bool>.Success(Presult.Message, false);





        }

        private Result<bool> VerifyPasswordWithPasswordPolicy(ApplicationUser user, string password)
        {
            if ((DateTimeOffset.Now.ToUnixTimeSeconds() - user.PasswordCreation) < MIN_PASSWORD_LENGTH_SECONDS)
            {
                return Result<bool>.Success("You reset your password too soon!", false);
            }
            else if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password) is PasswordVerificationResult.Success || _userManager.PasswordHasher.VerifyHashedPassword(user, user.PreviousPasswordHash ?? "", password) is PasswordVerificationResult.Success)
            {
                return Result<bool>.Success("Use a brand new password!", false);
            }
            return Result<bool>.Success("Success!", true);
        }

        


    }
}
