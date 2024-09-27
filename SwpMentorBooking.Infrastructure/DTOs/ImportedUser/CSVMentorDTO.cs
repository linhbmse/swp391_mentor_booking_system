using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Infrastructure.DTOs.ImportedUser
{
    public class CSVMentorDTO : CSVUserDTO
    {
        public string? MainProgrammingLanguage { get; set; }

        public string? AltProgrammingLanguage { get; set; }

        public string? Framework { get; set; }

        public string? Education { get; set; }

        public string? AdditionalContactInfo { get; set; }

        public string? Description {  get; set; }
    }
}
