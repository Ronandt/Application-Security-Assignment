using Microsoft.AspNetCore.Identity;

namespace Application_Security_Assignment.Identity
{
    public class ApplicationErrorDescriber : IdentityErrorDescriber
    {
       
        public override IdentityError DuplicateEmail(string email)
        {
            var error = base.DuplicateEmail(email);
            error.Description = "This email address has already been registered!";
            return error;
        }

        public override IdentityError DuplicateUserName(string userName)
        {
           var error = base.DuplicateUserName(userName);
            error.Description = "This email address has already been registered!";
           return error;
            
        }


    }
}
