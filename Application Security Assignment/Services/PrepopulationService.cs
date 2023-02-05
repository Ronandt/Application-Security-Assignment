using Application_Security_Assignment.Data.Models;
using Application_Security_Assignment.UiState;
using Microsoft.AspNetCore.Identity;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public class PrepopulationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICryptographyService _cryptographyService;
        private readonly IEncoderService _encoderService;
        private const string ADMIN_EMAIL = "admin@gmail.com";
        public PrepopulationService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ICryptographyService cryptographyService, IEncoderService encoderService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _cryptographyService = cryptographyService;
            _encoderService = encoderService;
        }

        public async Task PrepopulateAdmin()
        {
            //do the role later
            IdentityRole role = await _roleManager.FindByNameAsync("Admin");
            if (role is null)
            {
                 await _roleManager.CreateAsync(new IdentityRole("Admin"));
      
            }
        

            if (await _userManager.FindByEmailAsync(ADMIN_EMAIL) is null)
            {
                var newUser = new ApplicationUser()
                {
                    Email = ADMIN_EMAIL,
                    FullName = "Admin",
                    CreditCardNo = _cryptographyService.EncryptData("12345").Value,
                    Gender = Data.Enums.Gender.Male,
                    MobileNo = "20922",
                    DeliveryAddress = "Admin town",
                    UserName = ADMIN_EMAIL,
                    AboutMe = (await _encoderService.Encode("fsdiosfd")).Value,
                   
                    EmailConfirmed = true
                };



                var result = await _userManager.CreateAsync(newUser, "Admin123456!");
                if (result.Succeeded)
                {
                    if (!await _userManager.IsInRoleAsync(newUser, "Admin")) await _userManager.AddToRoleAsync(newUser, "Admin");
                }

            }
         
        }
    }
}
