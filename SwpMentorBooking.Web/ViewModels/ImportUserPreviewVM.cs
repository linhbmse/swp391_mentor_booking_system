using MimeKit.Cryptography;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Infrastructure.DTOs.ImportedUser;
using System.ComponentModel.DataAnnotations;


namespace SwpMentorBooking.Web.ViewModels
{
    [Serializable]
    public class ImportUserPreviewVM
    {

        public IEnumerable<(CSVStudentDTO Record, List<string> Errors)> Results { get; set; }
    }
}