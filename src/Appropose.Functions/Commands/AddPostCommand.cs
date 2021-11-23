using System;
using System.Threading;
using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;
using Appropose.Core.Interfaces;
using Azure.Storage.Blobs;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ToDoList.Functions.FluentErrors;
using Microsoft.Extensions.Configuration;
using static System.String;

namespace Appropose.Functions.Commands
{
    public class AddPostCommand : IRequest<Result>
    {
        public AddPostCommand(string title, string description, string localization, IFormFile image)
        {
            Title = title;
            Description = description;
            Localization = localization;
            Image = image;
        }

        public string Title { get; }
        public string Description { get;}
        public string Localization { get; }
        public IFormFile Image { get; }
    }

    public class AddPostCommandHandler : IRequestHandler<AddPostCommand, Result>
    {

        private readonly ILogger _logger;
        private readonly IPostRepository _repo;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IStorageService _storageService;

        public AddPostCommandHandler(ILogger logger, IPostRepository repo, BlobServiceClient blobServiceClient, IStorageService storageService)
        {
            _logger = logger;
            _repo = repo;
            _blobServiceClient = blobServiceClient;
            _storageService = storageService;
        }

        public async Task<Result> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            if (IsNullOrWhiteSpace(request.Description))
            {
                return Result.Fail(new ValidationError("Description must be specified"));
            }

            if (IsNullOrWhiteSpace(request.Title))
            {
                return Result.Fail(new ValidationError("Title must be specified"));
            }

            if (IsNullOrWhiteSpace(request.Localization))
            {
                return Result.Fail(new ValidationError("Localization must be specified"));
            }

            if (request.Image is null)
            {
                return Result.Fail(new ValidationError("Image is mandatory"));
            }

            var entity = PostEntity.Create(request.Title, request.Description, request.Localization);

            try
            {
                await _repo.AddItemAsync(entity);
                var fileName = $"{Guid.NewGuid()}-{request.Image.FileName}";
                await _storageService.UploadImageAsync(request.Image, fileName);
                entity.SetImageName(fileName);
                await _repo.UpdateItemAsync(entity.Id, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong during adding post");
                return Result.Fail(new RuntimeError("Something went wrong during adding new item"));
            }

            return Result.Ok();
        }
    }
}
