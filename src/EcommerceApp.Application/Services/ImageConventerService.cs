using System;
using System.IO;
using System.Threading.Tasks;
using EcommerceApp.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.Services
{
    public class ImageConverterService : IImageConverterService
    {
        public async Task<byte[]> GetByteArrayFromImageAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public string GetImageUrlFromByteArray(byte[] byteArray)
        {
            if (byteArray.Length > 0)
            {
                string imageData = Convert.ToBase64String(byteArray);
                return string.Format("data:image/jpg;base64,{0}", imageData);
            }
            else
            {
                return "No photo available";
            }
        }
    }
}
