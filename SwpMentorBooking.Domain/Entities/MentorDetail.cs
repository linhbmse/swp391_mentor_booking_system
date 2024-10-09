using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwpMentorBooking.Domain.Entities;

public partial class MentorDetail
{
    public int UserId { get; set; }
    [Display(Name = "Main Programming Language")]
    public string? MainProgrammingLanguage { get; set; }
    [Display(Name = "Alt. Programming Language")]
    public string? AltProgrammingLanguage { get; set; }

    public string? Framework { get; set; }

    public string? Education { get; set; }
    [Display(Name = "Contact Info")]
    public string? AdditionalContactInfo { get; set; }

    public int? BookingScore { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<MentorSchedule> MentorSchedules { get; set; } = new List<MentorSchedule>();
    [ValidateNever]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();

    public virtual ICollection<Specialization> Specs { get; set; } = new List<Specialization>();
}
