using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public interface ICryptographyService
    {
        public Result<string> EncryptData(string plaintextData);
        public Result<string> DecryptData(string encryptedData);
    }
}
