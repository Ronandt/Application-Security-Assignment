using Application_Security_Assignment.Data.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public class FilterSessionService: IFilterSessionService
    {
      
        private const string SESSION_VARIABLE = "UserEmail";
        public const int SESSION_TIMEOUT_IN_SECONDS = 300;


        public void StoreUserSession(string value, IHttpContextAccessor _context)
        {
            _context.HttpContext.Session.SetString(SESSION_VARIABLE, value);
        }

        public Result<bool> CheckUserSession(PageHandlerExecutingContext _context)
        {
            bool checkSession = _context.HttpContext.Session.GetString(SESSION_VARIABLE) is null;

            return Result<bool>.Success("Check of session is successful!", !checkSession);
            

          
        }

        public Result<bool> CheckUserSession(IHttpContextAccessor _context)
        {
            bool checkSession = _context.HttpContext.Session.GetString(SESSION_VARIABLE) is null;

            return Result<bool>.Success("Check of session is successful!", !checkSession);

        }

        public void ClearSession(IHttpContextAccessor _context)
        {
            _context.HttpContext.Session.Clear();
        }
    }
}
