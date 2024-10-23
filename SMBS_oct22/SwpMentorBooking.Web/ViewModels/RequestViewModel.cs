using System.ComponentModel.DataAnnotations;

namespace SwpMentorBooking.Web.ViewModels
{
    public class RequestViewModel
    {
        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
