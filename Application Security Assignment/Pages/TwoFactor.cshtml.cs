using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Application_Security_Assignment.Pages
{
    public class TwoFactorModel : PageModel
    {
        private readonly IEmailSenderService _emailSenderService;
        private readonly UserManager<ApplicationUser> _userManager;
        public TwoFactorModel(IEmailSenderService emailSenderService, UserManager<ApplicationUser> userManager)
        {
            _emailSenderService = emailSenderService;
            _userManager = userManager;
        }

        public async void OnGet(string email)
        {

            await _emailSenderService.SendEmailAsync(email, "Your two factor code", $"{await _userManager.GenerateTwoFactorTokenAsync(await _userManager.FindByEmailAsync(email), "Email")}");


        }


    }
}
