using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public interface ICaptchaService
    {
        public Task<Result<bool>> CaptchaPassed(string captchaResponse);
    }
}
