using Application_Security_Assignment.Data.Database.WebApp_Core_Identity.Model;
using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Data.Enums;
using Web.Utils;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace Application_Security_Assignment.Services
{
  
    public class LogService: ILogService
    {
        private readonly AuthDbContext _authDbContext;
        public LogService(AuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }

        public async Task LogUser(Actions action, string userEmail)
        {
            if(action == Actions.Login)
            {
                await _authDbContext.Log.AddAsync(new Log()
                {
                    Action="Login",
                    Description="User has logged in!",
                   LoggedEmailUser=userEmail,
               
                });
            }
            else if (action == Actions.Logout)
            {
                await _authDbContext.Log.AddAsync(new Log()
                {
                    Action = "Logout",
                    Description = "User has logged out!",
                    LoggedEmailUser = userEmail,
                 
                });
            }

            await _authDbContext.SaveChangesAsync();
        }

        public async Task<Result<List<Log>>> RetrieveLogs()
        {
            return Result<List<Log>>.Success("Successfully retrieved", _authDbContext.Log.OrderBy(x => x.Timestamp).ToList());
        }




    }
}
