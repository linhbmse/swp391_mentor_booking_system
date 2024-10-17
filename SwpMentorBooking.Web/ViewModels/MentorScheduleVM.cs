namespace SwpMentorBooking.Web.ViewModels
{
    public class MentorScheduleVM
    {
        public int Id { get; set; }
        public int MentorDetailId { get; set; }
        public int SlotId { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }

        // Include Slot details
        public string SlotStartTime { get; set; } // This assumes you have TimeOnly for start time
        public string SlotEndTime { get; set; }   // This assumes you have TimeOnly for end time
    }
}
