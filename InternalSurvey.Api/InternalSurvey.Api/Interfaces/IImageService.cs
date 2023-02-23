using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Interfaces
{
   public interface IImageService
    {
        string UploadImage(string image);

        string GetImageAsBase64String(string imagePath);

        string GetImage(string imagePath);

        string GetImageLocation(string imagePath);
    }
}
