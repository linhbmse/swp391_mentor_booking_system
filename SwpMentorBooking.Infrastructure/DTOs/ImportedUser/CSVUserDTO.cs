using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Infrastructure.DTOs.ImportedUser
{
    public class CSVUserDTO
    {
        [Name("Email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@([fpt\.edu|fe\.edu]+\.vn)$",
            ErrorMessage = "Email must be from fpt.edu.vn or fe.edu.vn domain.")]
        public string Email { get; set; }

        [Name("Full Name")]
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
        public string FullName { get; set; }

        [Name("Phone")]
        [RegularExpression(@"^$|^0\d{9}$", ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }
        [Name("Gender")]
        public string? Gender { get; set; }
    }
}
