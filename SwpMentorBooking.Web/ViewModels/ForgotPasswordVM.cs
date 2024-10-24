using System.ComponentModel.DataAnnotations;

namespace SwpMentorBooking.Web.ViewModels
{
    public class ForgotPasswordVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@([fpt\.edu|fe\.edu]+\.vn)$",
            ErrorMessage = "Email must be from fpt.edu.vn or fe.edu.vn domain.")]
        public string Email { get; set; }
    }

}
