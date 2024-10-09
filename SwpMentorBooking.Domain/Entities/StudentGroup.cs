using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwpMentorBooking.Domain.Entities;

public partial class StudentGroup
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    [Display(Name = "Group Name")]
    public string GroupName { get; set; } = null!;
    [Required]
    [Display(Name = "Topic")]
    public int TopicId { get; set; }

    public int WalletId { get; set; }

    public virtual ICollection<StudentDetail> StudentDetails { get; set; } = new List<StudentDetail>();

    public virtual Topic Topic { get; set; } = null!;

    public virtual Wallet Wallet { get; set; } = null!;
}
