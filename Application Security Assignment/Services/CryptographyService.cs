using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography.Xml;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public class CryptographyService : ICryptographyService
    {
        private readonly IDataProtectionProvider _dataProtectionProvider;
        private readonly IDataProtector _protector;


        public CryptographyService(string applicationName, string purpose)
        {
            _dataProtectionProvider = DataProtectionProvider.Create(applicationName);
            _protector = _dataProtectionProvider.CreateProtector(purpose);
        }


        public Result<string> EncryptData(string plaintextData)
        {
            if (plaintextData == null)
            {
                return Result<string>.Failure("Encrypted data is null");
            }
            return Result<string>.Success("Successfully sent encrypted data", _protector.Protect(plaintextData));
        }

        public Result<string> DecryptData(string encryptedData)
           
        {
            if(encryptedData == null)
            {
                return Result<string>.Failure("Encrypted data is null");
            }
            return Result<string>.Success("Successfully sent decrypted data", _protector.Unprotect(encryptedData));
        }

    }
}
