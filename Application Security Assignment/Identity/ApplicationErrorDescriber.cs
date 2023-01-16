using Microsoft.AspNetCore.Identity;

namespace Application_Security_Assignment.Identity
{
    public class ApplicationErrorDescriber : IdentityErrorDescriber
    {
       
        public override IdentityError DuplicateEmail(string email)
        {
            var error = base.DuplicateUserName(email);
            error.Description = "This email address has already been registered!";
            return error;
        }
    }
}
