using System;
using System.Collections.Generic;

namespace Email.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string? ProfilePhoto { get; set; }

    public string Role { get; set; } = null!;

    public bool IsFirstLogin { get; set; } = true;

    public bool IsActive { get; set; }

    public virtual AdminDetail? AdminDetail { get; set; }

    public virtual ICollection<Feedback> FeedbackGivenByNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackGivenToNavigations { get; set; } = new List<Feedback>();

    public virtual MentorDetail? MentorDetail { get; set; }

    public virtual StudentDetail? StudentDetail { get; set; }

    public virtual ICollection<UserActivity> UserActivities { get; set; } = new List<UserActivity>();

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
