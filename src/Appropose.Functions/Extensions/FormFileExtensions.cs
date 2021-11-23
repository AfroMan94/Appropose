using Microsoft.AspNetCore.Http;

namespace Appropose.Functions.Extensions
{
    public static class FormFileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            if(file.Length > 0 && file.ContentType.Contains("image"))
            {
                return true;
            }

            return false;
        }
    }
}
