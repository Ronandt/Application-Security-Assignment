﻿using Application_Security_Assignment.Data.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Net.Mail;
using System.Web;

namespace Application_Security_Assignment.Services
{
	public class EmailSenderService: IEmailSenderService
	{
        private readonly EmailCredentials credentials;
        private const string EMAIL_SENDER = "heckguy6969@gmail.com";
        private readonly string EMAIL_PASSWORD;
    
       

        public EmailSenderService( EmailCredentials emailCredentials)
        {
           
            credentials = emailCredentials ?? new EmailCredentials();
            EMAIL_PASSWORD = credentials.Password;

        }

     

        public async Task SendEmailAsync(string emailAddressReceiver, string subject, string message)
        {
            MailMessage newMail = new MailMessage();
            // use the Gmail SMTP Host
            SmtpClient client = new SmtpClient("smtp.gmail.com");

            // Follow the RFS 5321 Email Standard
            newMail.From = new MailAddress(EMAIL_SENDER, EMAIL_SENDER);

            newMail.To.Add(emailAddressReceiver);// declare the email subject

            newMail.Subject = subject; // use HTML for the email body

            newMail.IsBodyHtml = false;
            newMail.Body = message;
            client.UseDefaultCredentials = false;
            // enable SSL for encryption across channels
            client.EnableSsl = true;
     
            client.Port = 587;
            // Provide authentication information with Gmail SMTP server to authenticate your sender account
            client.Credentials = new System.Net.NetworkCredential(EMAIL_SENDER, EMAIL_PASSWORD);

            await client.SendMailAsync(newMail); // Send the constructed mail
            Console.WriteLine("Email Sent");



        }

        public async Task ExecuteEmailOperation(string apiKey, string subject, string message, string emailAddressReceiver)
        {
         
            SendGridClient client = new SendGridClient(apiKey);
            SendGridMessage clientMessage = new SendGridMessage()
            {
                From = new EmailAddress(EMAIL_SENDER, "Reset Password"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            clientMessage.AddTo(new EmailAddress(emailAddressReceiver));
            var test = await client.SendEmailAsync(clientMessage);
      ;

        }
    }
}
