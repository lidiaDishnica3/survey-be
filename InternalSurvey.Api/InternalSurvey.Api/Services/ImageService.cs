using InternalSurvey.Api.Helpers;
using InternalSurvey.Api.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ImageService> _logger;
        IConfiguration _configuration;

        public ImageService(IWebHostEnvironment env, ILogger<ImageService> logger, IConfiguration configuration)
        {
            _env = env;
            _logger = logger;
            _configuration = configuration;
        }

        public string UploadImage(string image)
        {
            try
            {
                var imageName = Path.GetRandomFileName() + ".jpg";
                var uploadPath = SaveFile(image, "Images", imageName);

                return uploadPath;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.UNEXPECTED_ERROR} ,cannot upload image: {image}");
                throw ex;
            }
        }

        private string SaveFile(string file, string folderName, string imageName)
        {
            try
            {
                string base64 = file.Substring(file.IndexOf(',') + 1);
                base64 = base64.Trim('\0');
                var imageBytes = Convert.FromBase64String(base64);
                var filePath = Path.Combine(_env.WebRootPath, folderName);
                var fullPath = Path.Combine(filePath, imageName);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                File.WriteAllBytes(fullPath, imageBytes);

                return $"{folderName}/{imageName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.UNEXPECTED_ERROR},cannot save : {imageName}.");
                throw ex;
            }
        }

        public string GetImageAsBase64String(string imagePath)
        {
            try
            {
                if (!String.IsNullOrEmpty(imagePath))
                {
                    var filePath = Path.Combine(_env.WebRootPath, imagePath);
                    var sourceStream = File.OpenRead(filePath);
                    byte[] array = new byte[1024];

                    using (var memoryStream = new MemoryStream())
                    {
                        sourceStream.CopyTo(memoryStream);
                        array = memoryStream.ToArray();
                    }

                    return Convert.ToBase64String(array);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.UNEXPECTED_ERROR} ImagePath: {imagePath}.");
                return null;
            }
        }

        public string GetImageLocation(string imagePath)
        {
            try
            {
                if (!String.IsNullOrEmpty(imagePath))
                {
                    var filePath = Path.Combine(_env.WebRootPath, imagePath);
                   
                    return filePath;
                }

                return "";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.UNEXPECTED_ERROR} ImagePath: {imagePath}.");
                return null;
            }
        }
        public string GetImage(string imagePath)
        {
            string img="";
            try
            {
                if (!String.IsNullOrEmpty(imagePath))
                {



                    var path = Path.Combine(_env.WebRootPath, imagePath);
                    if (System.IO.File.Exists(path))
                    {
                        var uri = new Uri(new Uri(_configuration.GetValue<string>("ApplicationsUrl:UrlApp")), imagePath);
                        img = uri.ToString();
                    }
                    else
                    {
                        img = "";
                    }                
                }
                return img;
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Messages.UNEXPECTED_ERROR} ImagePath: {imagePath}.");
                return null;
            }
}
    }
}
