using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Infrastructure.DTOs.ImportedUser
{
    public class CSVStudentDTO : CSVUserDTO
    {
        [Name("Student Code")]
        [Required(ErrorMessage = "Student Code is required.")]
        [RegularExpression(@"^[A-Za-z]{2}\d{6}$",
            ErrorMessage = "Student Code format is invalid.")]
        public string StudentCode { get; set; }
    }
}
