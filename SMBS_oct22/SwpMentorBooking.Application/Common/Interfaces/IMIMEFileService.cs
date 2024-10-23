
using Microsoft.AspNetCore.Http;

namespace SwpMentorBooking.Application.Common.Interfaces
{
    public interface IMIMEFileService
    {
        Task<string> UploadImage(IFormFile inputFile, string fileType);
        string GetFileType(string mimeType);
    }
}
