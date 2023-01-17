using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography.Xml;
using Web.Utils;

namespace Application_Security_Assignment.Services
{
    public class CryptographyService : ICryptographyService
    {

        private static readonly IDataProtectionProvider data = DataProtectionProvider.Create("Encrypt");
        private static readonly IDataProtector protector = data.CreateProtector("SecretKey");

        public static Result<string> EncryptData(string plaintextData)
        {
            if (plaintextData == null)
            {
                return Result<string>.Failure("Encrypted data is null");
            }
            return Result<string>.Success("Successfully sent encrypted data", protector.Protect(plaintextData));
        }

        public static Result<string> DecryptData(string encryptedData)
           
        {
            if(encryptedData == null)
            {
                return Result<string>.Failure("Encrypted data is null");
            }
            return Result<string>.Success("Successfully sent decrypted data", protector.Protect(encryptedData));
        }

    }
}
