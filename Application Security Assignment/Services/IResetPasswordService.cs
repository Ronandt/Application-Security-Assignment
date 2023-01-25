using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public interface IResetPasswordService
    {
        public Task<Result<bool>> ResetPassword(string email, string token, string password);
        public Task SendPasswordResetEmail(string email);

    }
}
