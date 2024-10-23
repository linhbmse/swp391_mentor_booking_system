using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SwpMentorBooking.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwpMentorBooking.Web.ViewModels
{
    public class StudentGroupVM
    {
        public StudentGroup? StudentGroup { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> TopicList { get; set; }
    }
}
