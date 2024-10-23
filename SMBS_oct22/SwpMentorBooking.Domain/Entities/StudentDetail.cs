using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwpMentorBooking.Domain.Entities;

public partial class StudentDetail
{
    public int UserId { get; set; }

    [Display(Name = "Student Code")]
    [Required(ErrorMessage = "Student Code is required.")]
    [RegularExpression(@"^[A-Za-z]{2}\d{6}$",
            ErrorMessage = "Student Code format is invalid.")]
    public string StudentCode { get; set; } = null!;

    public int? GroupId { get; set; }

    public bool IsLeader { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual StudentGroup? Group { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
    [ValidateNever]
    public virtual User User { get; set; } = null!;
}
