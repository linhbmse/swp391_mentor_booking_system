using System.ComponentModel.DataAnnotations;

namespace Email.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

}
