using System.Text;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public interface IEncoderService
    {
        public Task<Result<string>> Encode(string unencodedString);


        public Task<Result<string>> Decode(string encodedString);
    }
}
