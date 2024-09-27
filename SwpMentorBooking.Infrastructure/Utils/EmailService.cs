using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;

namespace SwpMentorBooking.Infrastructure.Utils
{
    public class EmailService
    {
        public IConfiguration Configuration { get; }
        private readonly string _smtpHost;
        private readonly string _sender;
        private readonly string _appPassword;
        //public EmailService(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //    _sender = Configuration.GetSection("EmailSettings").GetValue<string>("myMailAddress");
        //    _smtpHost = Configuration.GetSection("EmailSettings").GetValue<string>("smtpHostAddress");
        //}

        public EmailService(string smtpHost, string sender)
        {
            _smtpHost = smtpHost;
            _sender = sender;
        }
        public void sendEmail(string receiver, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress("HiepHX", _sender));
            mail.To.Add(new MailboxAddress("MinkTry", receiver));
            mail.Subject = subject;

            mail.Body = new TextPart("plain")
            {
                Text = $"{message}"
            };
            // 
            using (var client = new SmtpClient())
            {
                client.Connect(_smtpHost, 587, false);
                client.Authenticate(_sender, "vuwm qwpi spha jham");
                client.Send(mail);
                client.Disconnect(true);
            }

        }
    }
}
