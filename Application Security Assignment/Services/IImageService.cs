using Application_Security_Assignment.Data.Models;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public interface IImageService
    {


        public Task<Result<string>> StoreImage(IFormFile image, ApplicationUser user);
        public Task<Result<string>> RetrieveImage(ApplicationUser user);

    }
}
