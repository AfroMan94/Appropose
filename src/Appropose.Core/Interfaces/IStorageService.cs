using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Appropose.Core.Interfaces
{
    public interface IStorageService
    {
        Task UploadImageAsync(IFormFile file, string fileName);
    }
}
