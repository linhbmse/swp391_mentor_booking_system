using System.Collections.Generic;
using System;
using SwpMentorBooking.Web.ViewModels;

public class MentorBookingsVM
{
    public List<BookingDetailVM> Bookings { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
