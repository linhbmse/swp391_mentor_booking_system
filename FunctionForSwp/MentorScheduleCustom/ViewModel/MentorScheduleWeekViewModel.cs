using MentorScheduleCustom.Models;
using MentorScheduleCustom.ViewModel;
using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.ViewModel
{
    public class MentorScheduleWeekViewModel
    {
        public int MentorDetailId { get; set; } // The mentor whose schedule we are displaying
        public DateTime WeekStartDate { get; set; } // The start date of the week (Monday)
        public List<DailySchedule> DailySchedules { get; set; } // List of schedules for each day
    }

    public class DailySchedule
    {
        public DateTime Date { get; set; } // Date of the schedule (e.g., Monday, Tuesday)
        public List<MentorSchedule> MentorSchedules { get; set; } // Mentor's schedule for that day
    }
}
