using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IImageService
    {
        Task<string> ValidateImage(IFormFile imageFile);
        void DeleteImage(string imageUrl);
        string GenerateUrl(string imageUrl);
    }
}
