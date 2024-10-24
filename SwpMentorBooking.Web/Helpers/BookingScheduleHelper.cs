using SwpMentorBooking.Application.Common.Interfaces;
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
        public static bool IsBookingCompletable(Booking booking)
        {
            return booking.Status == "confirmed";
        }
        public static bool IsBookingFeedbackable(Booking booking, User user, IUnitOfWork unitOfWork)
        {
            if (booking.Status is not "completed")
            {
                return false;
            }

            // Check if the user has already given feedback for this booking
            var existingFeedback = unitOfWork.Feedback.Get(f => f.BookingId == booking.Id && f.GivenBy == user.Id);

            return (existingFeedback == null);
        }
    }
}