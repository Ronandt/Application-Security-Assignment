using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using System.Runtime;
using System.Security.Claims;

namespace Application_Security_Assignment.Pages
{
    public class ExternalLoginModel : PageModel
    {
  
        private readonly AuthenticationService _authenticationService;

        public ExternalLoginModel(AuthenticationService authenticationService)
        {
     
            _authenticationService = authenticationService;
        }
        public async Task<IActionResult> OnGet()
        {
            var loginResult = await _authenticationService.ExternalLogin();
           if (loginResult.Value)
            {
                TempData["success"] = loginResult.Message;
                return Redirect("/");
            }
            return Redirect("/login");

   
        }
    }
}
