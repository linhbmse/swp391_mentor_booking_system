namespace SwpMentorBooking.Web.ViewModels
{
    public class BookingDetailVM
    {
        public int BookingId { get; set; }
        public string? GroupName { get; set; }
        public string MentorName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public int? SlotId { get; set; }
        public TimeOnly SlotStartTime { get; set; }
        public TimeOnly SlotEndTime { get; set; }
        public string? Note { get; set; }
        public int BookingCost { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public string? TopicName { get; set; }
        public bool IsPastBooking {  get; set; }
        public bool IsApprovable { get; set; } = false;
    }
}
