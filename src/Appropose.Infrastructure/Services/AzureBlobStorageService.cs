using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Appropose.Core.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace Appropose.Infrastructure.Services
{
    public class AzureBlobStorageService : IStorageService
    {
        private readonly BlobContainerClient _blobContainerClient;

        public AzureBlobStorageService(IConfiguration config)
        {
            string connectionString = config.GetConnectionString("StorageConnectionString");
            _blobContainerClient = new BlobContainerClient(connectionString, "images");
            _blobContainerClient.CreateIfNotExists();
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            var fileName = Guid.NewGuid().ToString();
            var blob = _blobContainerClient.GetBlobClient(fileName);
            await blob.UploadAsync(file.OpenReadStream());
            return fileName;
        }
    }
}
