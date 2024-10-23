using SwpMentorBooking.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Infrastructure.Utils
{
    public class UtilService : IUtilService
    {
        public ICSVFileService CSV { get; }
        public IAutoMapperService AutoMapper { get; }
        public IEmailService Email { get; }
        public IPasswordGeneratorService PasswordGenerator { get; }
        public IMIMEFileService MIMEFile { get; }

        public IStringManipulationService StringManipulation { get; }

        public UtilService(
            ICSVFileService csvFileService,
            IAutoMapperService autoMapperService,
            IEmailService emailService,
            IPasswordGeneratorService passwordGeneratorService,
            IMIMEFileService mimeFileService,
            IStringManipulationService stringManipulation)
        {
            CSV = csvFileService;
            AutoMapper = autoMapperService;
            Email = emailService;
            PasswordGenerator = passwordGeneratorService;
            MIMEFile = mimeFileService;
            StringManipulation = stringManipulation;
        }
    }
}
