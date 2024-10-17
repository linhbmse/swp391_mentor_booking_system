using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Web.ViewModels
{
    public class BookingScheduleDetailsVM
    {
        public int GroupId { get; set; }
        public int MentorId { get; set; }
        public string GroupName { get; set; }
        public string TopicName { get; set; }
        public string MentorName { get; set; }
        public List<string>? MentorSpecializations { get; set; }
        public List<MentorSchedule>? AvailableSchedules { get; set; }
        public int SelectedScheduleId { get; set; }
    }
}
