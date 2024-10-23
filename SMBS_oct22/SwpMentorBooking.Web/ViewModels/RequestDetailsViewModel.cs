//using Azure.Core;
using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Web.ViewModels
{
    public class RequestDetailsViewModel
    {
        public Request Request { get; set; }  // This is crucial to hold the request details.
        public string Action { get; set; }    // Optional: to hold the action type ("approve", "reject").
    }

}
