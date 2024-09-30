using System.ComponentModel.DataAnnotations;

namespace SwpMentorBooking.Web.ViewModels
{
    public class LoginVM
    {      
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@([fpt\.edu|fe\.edu]+\.vn)$",
            ErrorMessage = "Email must be from fpt.edu.vn or fe.edu.vn domain.")]
        public string UserEmail { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
