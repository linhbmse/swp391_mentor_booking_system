using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SwpMentorBooking.Infrastructure.Data;
using SwpMentorBooking.Infrastructure.DTOs;

namespace SwpMentorBooking.Infrastructure.Utils
{
    public class MIMEFileService
    {
        public IConfiguration Configuration { get; }
        private CloudinarySettings setting;
        private Cloudinary cloudinary;
        private readonly ApplicationDbContext _context;

        public MIMEFileService(IConfiguration configuration, ApplicationDbContext context)
        {
            Configuration = configuration;
            setting = Configuration.GetSection("CloudinarySettings").Get<CloudinarySettings>();
            Account acc = new Account(setting.CloudName, setting.ApiKey, setting.ApiSecret);
            cloudinary = new Cloudinary(acc);
            this._context = context;
        }

        public async Task<string> UploadImage(IFormFile inputFile, string fileType)
        {
            if (fileType == "Images")
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(inputFile.FileName.Replace(" ", ""), inputFile.OpenReadStream()),
                    PublicId = Guid.NewGuid().ToString()
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                // Check if uploadResult's status code = OK (200) => upload successful
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {   // return image's URL
                    return uploadResult.Url.ToString();
                }
            }

            else if (fileType == "Videos")
            {       // the same applies to Videos
                var uploadParams = new VideoUploadParams()
                {
                    File = new FileDescription(inputFile.FileName.Replace(" ", ""), inputFile.OpenReadStream()),
                    PublicId = Guid.NewGuid().ToString()
                };

                var uploadResult = await cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return uploadResult.Url.ToString();
                }
            }

            else
            {
                throw new Exception("Cannot upload file to cloudinary");
            }

            return "";
        }
        public string GetFileType(string mimeType)
        {
            string fileType = "";
            if (mimeType.Contains("image"))
            {
                fileType = "Images";
            }

            if (mimeType.Contains("video"))
            {
                fileType = "Videos";
            }

            return fileType;
        }
    }
}
