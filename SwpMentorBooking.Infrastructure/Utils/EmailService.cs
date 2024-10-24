using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using SwpMentorBooking.Application.Common.Interfaces;

namespace SwpMentorBooking.Infrastructure.Utils
{
    public class EmailService : IEmailService
    {
        public IConfiguration Configuration { get; }
        private readonly string _smtpHostAddress;
        private readonly string _senderName;
        private readonly string _senderMailAddress;
        private readonly string _appPassword;

        public EmailService(IConfiguration configuration)
        {
            Configuration = configuration;
            _senderName = Configuration.GetSection("EmailSettings").GetValue<string>("SenderName");
            _senderMailAddress = Configuration.GetSection("EmailSettings").GetValue<string>("SenderMailAddress");
            _smtpHostAddress = Configuration.GetSection("EmailSettings").GetValue<string>("SmtpHostAddress");
            _appPassword = Configuration.GetSection("EmailSettings").GetValue<string>("AppPassword");
        }

        //public EmailService(string smtpHost, string sender)
        //{
        //    _smtpHost = smtpHost;
        //    _sender = sender;
        //}
        public async Task SendEmail(string receiverMail, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_senderName, _senderMailAddress));
            mail.To.Add(new MailboxAddress("MinkTry", receiverMail));

            mail.Subject = subject;

            mail.Body = new TextPart("html")
            {
                Text = $"{message}"
            };
            // 
            using (var client = new SmtpClient())
            {
                client.Connect(_smtpHostAddress, 587, false);
                client.Authenticate(_senderMailAddress, _appPassword);
                await client.SendAsync(mail);
                client.Disconnect(true);
            }

        }
    }
}
