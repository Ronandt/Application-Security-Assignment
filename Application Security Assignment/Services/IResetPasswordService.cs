using Application_Security_Assignment.Data.Models;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public interface IResetPasswordService
    {
        public Task<Result<bool>> ResetPassword(string email, string token, string password);
        public Task SendPasswordResetEmail(string email);
        public Task<Result<bool>> ResetPassword(ApplicationUser user, string currentPassword, string newPassword);

    }
}
