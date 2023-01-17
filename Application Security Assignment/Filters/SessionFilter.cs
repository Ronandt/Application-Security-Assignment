using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application_Security_Assignment.Filters
{
    public class SessionAsyncFilter : IAsyncPageFilter
    {
        public Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (context.HttpContext.Session.GetString("UserId") is null && context.HttpContext.Request.Path != "/login")
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
