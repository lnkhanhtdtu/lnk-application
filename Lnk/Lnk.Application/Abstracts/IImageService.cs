using Microsoft.AspNetCore.Http;

namespace Lnk.Application.Abstracts
{
    public interface IImageService
    {
        Task<bool> SaveImage(List<IFormFile> images, string path, string? defaultName);
    }
}
