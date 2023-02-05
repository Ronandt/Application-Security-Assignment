using System.Text;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public class EncoderService: IEncoderService
    {
        public async Task<Result<string>> Encode(string unencodedString)
        {
            if(unencodedString is null)
            {
                return Result<string>.Failure("No proper character");
            }
            return Result<string>.Success("Converted!", Convert.ToBase64String(Encoding.UTF8.GetBytes(unencodedString)));
        }

        public async Task<Result<string>> Decode(string encodedString)
        {
            if (encodedString is null)
            {
                return Result<string>.Failure("No proper character");
            }
            return Result<string>.Success("Converted!", Encoding.UTF8.GetString(Convert.FromBase64String(encodedString)));
        }

       
    }
}
