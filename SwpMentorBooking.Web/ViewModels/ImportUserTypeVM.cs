using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SwpMentorBooking.Web.ViewModels
{
    public class ImportUserTypeVM
    {
        public IFormFile InputFile { get; set; }
        [ValidateNever]
        public string SelectedUserType { get; set; }
        public IEnumerable<SelectListItem> UserType { get; set; }
    }
}
