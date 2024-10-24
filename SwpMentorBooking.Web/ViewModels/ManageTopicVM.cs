using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Web.ViewModels
{
    public class ManageTopicVM
    {
        public List<Topic>? Topics { get; set; } = new List<Topic>();

        public Topic? NewTopic { get; set; }
    }
}
