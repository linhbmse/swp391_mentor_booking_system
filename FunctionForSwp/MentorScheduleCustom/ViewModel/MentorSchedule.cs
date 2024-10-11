using System;
namespace MentorScheduleCustom.ViewModel
{


    public class MentorSchedule
    {
        public int Id { get; set; }
        public int MentorDetailId { get; set; }
        public int SlotId { get; set; }
        public DateOnly Date { get; set; }
        public string Status { get; set; }

        public virtual Slot Slot { get; set; }
    }

    public class Slot
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

  

}
