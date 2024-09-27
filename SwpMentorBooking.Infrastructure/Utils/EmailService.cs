using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace SwpMentorBooking.Infrastructure.Utils
{
    public class EmailService
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
        }

        //public EmailService(string smtpHost, string sender)
        //{
        //    _smtpHost = smtpHost;
        //    _sender = sender;
        //}
        public void sendEmail(string receiverMail, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_senderName, _senderMailAddress));
            mail.To.Add(new MailboxAddress("MinkTry", receiverMail));

            mail.Subject = subject;

            mail.Body = new TextPart("plain")
            {
                Text = $"{message}"
            };
            // 
            using (var client = new SmtpClient())
            {
                client.Connect(_smtpHostAddress, 587, false);
                client.Authenticate(_senderMailAddress, _appPassword);
                client.Send(mail);
                client.Disconnect(true);
            }

        }
    }
}
