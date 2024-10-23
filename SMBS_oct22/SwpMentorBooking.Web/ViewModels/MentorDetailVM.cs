using SwpMentorBooking.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwpMentorBooking.Web.ViewModels
{
    public class MentorDetailVM
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        public string Gender {  get; set; }
        [NotMapped]
        public IFormFile? ProfilePhoto { get; set; }
        public List<Skill>? Skills {  get; set; }
        public List<Specialization>? Specializations { get; set; }

        [Display(Name = "Main Programming Language")]
        public List<string>? MainProgrammingLanguage { get; set; }

        [Display(Name = "Alt. Programming Language")]
        public List<string>? AltProgrammingLanguage { get; set; }

        public List<string>? Framework { get; set; }

        public List<string>? Education { get; set; }

        [Display(Name = "Additional Contact Info")]
        public List<string>? AdditionalContactInfo { get; set; }
        [Display(Name = "Booking Score")]
        public int? BookingScore { get; set; }

        public string? Description { get; set; }
    }
}
