using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.Services;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;


namespace Application_Security_Assignment.Pages
{
    [ValidateAntiForgeryToken]
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public RegisterUiState RegisterUiState { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IImageService _imageService;
        private readonly ICryptographyService _cryptographyService;
        private readonly ICaptchaService _captchaService;
        private readonly IEncoderService _encoderService;

        public RegisterModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IImageService imageService, ICryptographyService cryptographyService, ICaptchaService captchaService, IEncoderService encoderService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _imageService = imageService;
            _cryptographyService = cryptographyService;
            _captchaService = captchaService;
            _encoderService = encoderService;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {

            if(ModelState.IsValid)
            {

                if(!(await _captchaService.CaptchaPassed(Request.Form["token"])).Value)
                {
                    ModelState.AddModelError("", "You have failed the CAPTCHA. Try again.");
                    return Page();

                }
            

                var newUser = new ApplicationUser()
                {
                    Email = RegisterUiState.Email,
                    FullName = RegisterUiState.FullName,
                    CreditCardNo = _cryptographyService.EncryptData(RegisterUiState.CreditCardNo).Value,
                    Gender = RegisterUiState.gender,
                    MobileNo = RegisterUiState.MobileNo,
                    DeliveryAddress = RegisterUiState.DeliveryAddress,
                    UserName = RegisterUiState.Email,
                    AboutMe = (await _encoderService.Encode(RegisterUiState.AboutMe)).Value,
                    TwoFactorEnabled = true,
                    EmailConfirmed = true
                };
              

                var result = await _userManager.CreateAsync(newUser, RegisterUiState.Password);
                if(result.Succeeded)
                {
                    await _imageService.StoreImage(RegisterUiState?.Image, newUser);
                    return Redirect("/login");
                } else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return  Page();
                }
              
            }



            return Page();
        }
    }
}
