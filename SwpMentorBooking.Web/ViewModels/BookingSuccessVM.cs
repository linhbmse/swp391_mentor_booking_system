namespace SwpMentorBooking.Web.ViewModels
{
    public class BookingSuccessVM
    {
        public int BookingId { get; set; }
        public int SlotId { get; set; }
        public string GroupName { get; set; }
        public string MentorName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeOnly SlotStartTime { get; set; }
        public TimeOnly SlotEndTime { get; set; }
        public string? Note { get; set; }
        public int BookingCost { get; set; }
        public DateTime Timestamp { get; set; }
        public int? GroupId { get; set; }
        public int MentorId { get; set; }
    }
}
