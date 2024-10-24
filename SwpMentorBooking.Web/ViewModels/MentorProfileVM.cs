using SwpMentorBooking.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwpMentorBooking.Web.ViewModels
{
    public class MentorProfileVM
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        [NotMapped]
        public IFormFile? ProfilePhoto { get; set; }
        public List<Skill>? Skills { get; set; }
        public List<Specialization>? Specializations { get; set; }
        public List<string>? MainProgrammingLanguage { get; set; }
        public List<string>? AltProgrammingLanguage { get; set; }
        public List<string>? Framework { get; set; }
        public List<string>? Education { get; set; }
        public int BookingScore { get; set; }
        public string Description { get; set; }

        public bool IsStudentGroupLeader { get; set; }
    }
}
