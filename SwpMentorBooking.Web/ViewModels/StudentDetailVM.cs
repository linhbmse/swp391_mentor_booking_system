using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwpMentorBooking.Web.ViewModels
{
    public class StudentDetailVM
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        public string Gender { get; set; }
        [NotMapped]
        public IFormFile? ProfilePhoto { get; set; }
        [Display(Name = "Student Code")]
        public string StudentCode { get; set; }
    }
}
