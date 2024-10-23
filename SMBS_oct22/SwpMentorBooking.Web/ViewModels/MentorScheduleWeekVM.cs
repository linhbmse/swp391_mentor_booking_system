using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Web.ViewModels
{
    public class MentorScheduleWeekVM
    {
        public int MentorId { get; set; }
        public string MentorFullName { get; set; }
        public DateTime WeekStartDate { get; set; }
        public List<DailySchedule>? DailySchedules { get; set; }
        public List<Slot>? Slots { get; set; } = new List<Slot>();
    }

    public class DailySchedule
    {
        public DateTime Date { get; set; }
        public List<MentorScheduleVM> MentorSchedules { get; set; } // Update to MentorScheduleVM
        public bool IsPastDay { get; set; }
        public bool IsPastSlot { get; set; }
    }
}
