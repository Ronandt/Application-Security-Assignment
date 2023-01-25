namespace Application_Security_Assignment.Services
{
    public class LockoutConstants
    {
        public const int MAX_FAILED_ATTEMPTS = 10;
        public const int LOCKOUT_TIMESPAN_IN_MINUTES = 1;
        public const bool ALLOWED_FOR_NEW_USERS = true;
    }
}
