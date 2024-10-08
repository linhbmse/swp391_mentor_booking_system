namespace Email
{
    using System.Net;
    using System.Net.Mail;

    public static class SmtpClientFactory
    {
        public static SmtpClient CreateSmtpClient(string host, int port, string username, string password, bool enableSsl = true)
        {
            var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl,
            };

            return smtpClient;
        }
    }

}
