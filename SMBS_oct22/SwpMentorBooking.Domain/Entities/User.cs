using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwpMentorBooking.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    [Display(Name = "Full Name")]
    [Required(ErrorMessage = "Full Name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Full Name must be between 2 and 100 characters.")]
    public string FullName { get; set; } = null!;
    [Display(Name = "Phone Number")]
    [RegularExpression(@"^$|^0\d{9}$", ErrorMessage = "Invalid phone number.")]
    public string? Phone { get; set; }

    public string? Gender { get; set; }
    [Display(Name = "Profile Photo")]
    public string? ProfilePhoto { get; set; }

    [NotMapped]
    public IFormFile? Image { get; set; }

    public string Role { get; set; } = null!;

    public bool IsFirstLogin { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public virtual AdminDetail? AdminDetail { get; set; }

    public virtual ICollection<Feedback> FeedbackGivenByNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackGivenToNavigations { get; set; } = new List<Feedback>();

    public virtual MentorDetail? MentorDetail { get; set; }

    public virtual StudentDetail? StudentDetail { get; set; }

    public virtual ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
