using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Application_Security_Assignment.Services
{
    public class RegexConstants
    {
        public const string PASSWORD_PATTERN = "(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\\s).{12,}";
        public static readonly Regex PASSWORD_REGEX = new Regex(PASSWORD_PATTERN);

        public const string DISALLOW_SPECIAL_CHARACTERS_PATTERN = @"^[a-zA-Z0-9]+$";
        public static readonly Regex DISALLOW_SPECIAL_CHARACTERS_REGEX = new Regex(DISALLOW_SPECIAL_CHARACTERS_PATTERN);
    }
}
