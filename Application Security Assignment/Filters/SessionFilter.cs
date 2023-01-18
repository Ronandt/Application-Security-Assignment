using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application_Security_Assignment.Filters
{
    public class SessionAsyncFilter : IAsyncPageFilter
    {
       private readonly IFilterSessionService _filterSessionService;
        private readonly List<string> allowedPaths = new List<string>()
        {
            "/login",
            "/register",
            "/error"
           
        };
    
        public SessionAsyncFilter(IFilterSessionService filterSessionService)
        {
            _filterSessionService = filterSessionService;
          
        }

        public Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (!_filterSessionService.CheckUserSession(context).Value && !allowedPaths.Any(x=> x.ToString() == context.HttpContext.Request.Path))
            {
              
              
                context.Result = new RedirectToPageResult("/login");
                return Task.CompletedTask;
            }
            return next();
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
