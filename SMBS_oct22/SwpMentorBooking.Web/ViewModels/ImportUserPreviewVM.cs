using MimeKit.Cryptography;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Infrastructure.DTOs.ImportedUser;
using System.ComponentModel.DataAnnotations;


namespace SwpMentorBooking.Web.ViewModels
{
    [Serializable]
    public class ImportUserPreviewVM<T>
    {

        public IEnumerable<(T Record, List<string> Errors)> Results { get; set; }
        public List<string> ImportErrors { get; set; } = new List<string>();
    }
}