using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Application.Common.Interfaces
{
    public interface IUtilService
    {
        ICSVFileService CSV { get; }
        IAutoMapperService AutoMapper { get; }
        IPasswordGeneratorService PasswordGenerator { get; }
        IMIMEFileService MIMEFile { get; }
        IEmailService Email { get; }

        IStringManipulationService StringManipulation { get; }
    }
}
