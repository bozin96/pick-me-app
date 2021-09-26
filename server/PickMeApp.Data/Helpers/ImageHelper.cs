using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PickMeApp.Application.Helpers
{
    public static class ImageHelper
    {
        public static byte[] ImageToBytes(IFormFile image)
        {
            byte[] fileBytes = null;
            if (image != null && image.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    image.CopyTo(ms);
                    fileBytes = ms.ToArray();
                }
            }
            return fileBytes;
        }

        public static string ImageToBase64(IFormFile image)
        {
            byte[] fileBytes;
            string imageBase64 = "";
            if (image != null && image.Length > 0)
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
    
        public static byte[] ImageFromBase64(string image)
        {
            if (!string.IsNullOrEmpty(image))
            {
                string s = image.Replace('-', '+').Replace('_', '/').PadRight(4 * ((image.Length + 3) / 4), '=');
                return Convert.FromBase64String(s);
            }
            return null;
        }
    }
}
