using MentorScheduleCustom.Models;
using MentorScheduleCustom.ViewModel;
using System;
using System.Collections.Generic;

namespace MentorScheduleCustom.ViewModel
{
    public class MentorScheduleWeekViewModel
    {
        public int MentorDetailId { get; set; }
        public DateTime WeekStartDate { get; set; }
        public List<DailySchedule> DailySchedules { get; set; }
    }

    public class DailySchedule
    {
        public DateTime Date { get; set; }
        public List<MentorScheduleViewModel> MentorSchedules { get; set; } // Update to MentorScheduleViewModel
    }
}
