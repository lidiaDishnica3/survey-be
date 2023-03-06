using InternalSurvey.Api.Helpers.Email.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;

namespace InternalSurvey.Api.Helpers.Email.Services
{
    public class EmailService: IEmailService
    {
        private readonly Email _email;
        private readonly ILogger<Email> _logger;

        public EmailService(IOptions<Email> email, ILogger<Email> logger)
        {
            _email = email.Value;
            _logger = logger;
        }

        public async Task SendEmail(string email, string subject, string message)
        {
            try
            {
                var mimeMessage = new MimeMessage();
                var builder = new BodyBuilder();

                mimeMessage.From.Add(new MailboxAddress(_email.Sender,email));
                mimeMessage.To.Add(new MailboxAddress(email,email));
                mimeMessage.Subject = subject;
                builder.HtmlBody = message;
                mimeMessage.Body = builder.ToMessageBody();

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(_email.MailServer, _email.MailPort);
                    await client.AuthenticateAsync(_email.Sender, _email.Password);
                    await client.SendAsync(mimeMessage);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, Messages.UNEXPECTED_ERROR);
                throw ex;
            }
        }
    }
}
