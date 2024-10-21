using SwpMentorBooking.Domain.Entities;
using System;

namespace SwpMentorBooking.Web.Helpers
{
    public static class BookingScheduleHelper
    {
        public static bool IsBookingInPast(MentorSchedule schedule)
        {
            return schedule.Date < DateOnly.FromDateTime(DateTime.Now) ||
                (schedule.Date == DateOnly.FromDateTime(DateTime.Now) &&
                 schedule.Slot.EndTime < TimeOnly.FromDateTime(DateTime.Now));
        }

        public static bool IsBookingApprovable(Booking booking)
        {
            return booking.Status == "pending" && !IsBookingInPast(booking.MentorSchedule);
        }
    }
}