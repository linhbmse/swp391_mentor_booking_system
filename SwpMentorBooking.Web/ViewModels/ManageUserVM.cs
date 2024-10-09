using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Web.ViewModels
{
    public class ManageUserVM
    {
        public IEnumerable<User> Students { get; set; }
        public IEnumerable<User> Mentors { get; set; }
    }
}
