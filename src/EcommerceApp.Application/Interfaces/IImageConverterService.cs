using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EcommerceApp.Application.Interfaces
{
    public interface IImageConverterService
    {
        Task<byte[]> GetByteArrayFromImageAsync(IFormFile file);
        string GetImageUrlFromByteArray(byte[] byteArray);
    }
}
