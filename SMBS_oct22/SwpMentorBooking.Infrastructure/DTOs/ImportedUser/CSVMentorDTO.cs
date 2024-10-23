using CsvHelper.Configuration.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SwpMentorBooking.Infrastructure.DTOs.ImportedUser
{
    public class CSVMentorDTO : CSVUserDTO
    {
        public string? Skills { get; set; }

        public string? Specialization { get; set; }
        [Name("Main Prog. Language")]
        public string? MainProgrammingLanguage { get; set; }
        [Name("Alt Prog. Language")]
        public string? AltProgrammingLanguage { get; set; }

        public string? Framework { get; set; }

        public string? Education { get; set; }
        [Name("Additional Contact Info")]
        public string? AdditionalContactInfo { get; set; }

        public string? Description { get; set; }
        [ValidateNever]
        public List<string> MainProgrammingLanguageList { get; set; } = new List<string>();
        [ValidateNever]
        public List<string> AltProgrammingLanguageList { get; set; } = new List<string>();
        [ValidateNever]
        public List<string> FrameworkList { get; set; } = new List<string>();

    }
}
