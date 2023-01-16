using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace Application_Security_Assignment.Services
{
    public class RegexConstants
    {
        public static readonly Regex PASSWORD = new Regex("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])(?!.*\\s).{12,}");
    }
}
