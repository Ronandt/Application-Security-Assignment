using Application_Security_Assignment.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace Application_Security_Assignment.Data.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FullName { get; set; }
        public string CreditCardNo { get; set; }
        public Gender Gender { get; set; }
        public string MobileNo { get; set; }
        public string DeliveryAddress { get; set; }
        public string ImageURL { get; set; }
        public string AboutMe { get; set; }
    }
}
