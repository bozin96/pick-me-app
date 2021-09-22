using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PickMeApp.Application.Helpers
{
    public static class ImageHelper
    {
        public static string ImageToBase64(IFormFile image)
        {
            byte[] fileBytes;
            string imageBase64 = "";
            if (image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    fileBytes = ms.ToArray();
                    imageBase64 = Convert.ToBase64String(fileBytes);
                }
            }
            return imageBase64;
        }
    }
}
