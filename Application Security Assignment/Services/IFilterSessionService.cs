using Application_Security_Assignment.Data.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public interface IFilterSessionService
    {

        public void StoreUserSession(string value, IHttpContextAccessor _context);
       public Result<bool> CheckUserSession(PageHandlerExecutingContext _context);
        public Result<bool> CheckUserSession(IHttpContextAccessor _context);
        public void ClearSession(IHttpContextAccessor _context);
    }
}
