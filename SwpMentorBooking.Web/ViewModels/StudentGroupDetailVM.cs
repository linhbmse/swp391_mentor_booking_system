using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Web.ViewModels
{
    public class StudentGroupDetailVM
    {
        public StudentDetail Student { get; set; }
        public StudentGroup? StudentGroup { get; set; }

        public List<StudentDetail>? GroupMembers { get; set; } = new List<StudentDetail>();
    }
}
