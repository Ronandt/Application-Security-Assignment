using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application_Security_Assignment.Filters
{

    public class SecurityFilter : ResultFilterAttribute
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public SecurityFilter(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            
            if((DateTimeOffset.Now.ToUnixTimeSeconds() - (await _userManager.GetUserAsync(context.HttpContext.User)).PasswordCreation) > ResetPasswordService.MAX_PASSWORD_LENGTH_SECONDS)
            {
                
                context.Result = new RedirectToPageResult("/changepassword");
             
                
            }
            await base.OnResultExecutionAsync(context, next);

        }

    }
}