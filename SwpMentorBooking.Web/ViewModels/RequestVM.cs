using SwpMentorBooking.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SwpMentorBooking.Web.ViewModels
{
    public class RequestVM
    {
        public int LeaderId { get; set; }
        [Display(Name = "Group")]
        public string? GroupName { get; set; }
        public Request? Request { get; set; }
    }
}
