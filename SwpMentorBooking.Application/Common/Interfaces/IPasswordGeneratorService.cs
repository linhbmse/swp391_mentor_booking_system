using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Application.Common.Interfaces
{
    public interface IPasswordGeneratorService
    {
        string GeneratePassword(int length = 12);
    }
}
