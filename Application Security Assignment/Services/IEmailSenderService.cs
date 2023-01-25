namespace Application_Security_Assignment.Services
{
	public interface IEmailSenderService
	{
        public Task SendEmailAsync(string emailAddressReceiver, string subject, string message);
        public Task ExecuteEmailOperation(string apiKey, string subject, string message, string emailAddressReceiver);
    }
}
