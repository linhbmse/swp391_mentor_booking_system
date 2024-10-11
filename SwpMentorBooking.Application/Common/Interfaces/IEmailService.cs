using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Application.Common.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string receiverMail, string subject, string message);
    }
}
