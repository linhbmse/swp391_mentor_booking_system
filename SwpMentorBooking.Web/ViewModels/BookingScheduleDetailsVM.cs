using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SwpMentorBooking.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace SwpMentorBooking.Web.ViewModels
{
    public class BookingScheduleDetailsVM
    {
        public int GroupId { get; set; }
        public int LeaderId { get; set; }
        public int MentorId { get; set; }
        public int MentorScheduleId { get; set; }
        public int SlotId { get; set; }
        public string GroupName { get; set; }
        public string GroupTopic { get; set; }
        public string MentorName { get; set; }
        public DateTime ScheduleDate { get; set; }
        public TimeOnly SlotStartTime { get; set; }
        public TimeOnly SlotEndTime { get; set; }
        public string? Note { get; set; }

        public int? WalletBalance { get; set; }
        public int? BookingCost { get; set; }

        [SufficientBalance(ErrorMessage = "Insufficient balance to make a schedule booking.")]
        public int? BalanceAfter => WalletBalance.HasValue && BookingCost.HasValue
            ? Math.Max(WalletBalance.Value - BookingCost.Value, 0)
            : null;

        [ValidateNever]
        public IEnumerable<SelectListItem> SlotList { get; set; }


        public class SufficientBalanceAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is int balanceAfter && balanceAfter > 0)
                {
                    return ValidationResult.Success;
                }
                // If the balance is insufficient, it should be 0
                return new ValidationResult(ErrorMessage);
            }
        }
    }
}
