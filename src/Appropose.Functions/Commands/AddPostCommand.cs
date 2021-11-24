using System;
using System.Threading;
using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;
using Appropose.Core.Interfaces;
using Appropose.Functions.Extensions;
using Appropose.Functions.FluentErrors;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using static System.String;

namespace Appropose.Functions.Commands
{
    public class AddPostCommand : IRequest<Result>
    {
        public AddPostCommand(string title, string question, string description, float latitude, float longitude, string userId, IFormFile image)
        {
            Title = title;
            Question = question;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            UserId = userId;
            Image = image;
        }

        public string Title { get; }
        public string Question { get; }
        public string Description { get;}
        public string UserId { get; }
        public float? Latitude { get; }
        public float? Longitude { get; }
        public IFormFile Image { get; }
    }

    public class AddPostCommandHandler : IRequestHandler<AddPostCommand, Result>
    {
        private readonly ILogger _logger;
        private readonly IPostRepository _repo;
        private readonly IStorageService _storageService;
        private readonly IConfiguration _configuration;

        public AddPostCommandHandler(ILogger logger, IPostRepository repo, IStorageService storageService, IConfiguration configuration)
        {
            _logger = logger;
            _repo = repo;
            _storageService = storageService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            if (!request.Image.IsImage())
            {
                return Result.Fail(new ValidationError("Image is not valid"));
            }

            if (IsNullOrWhiteSpace(request.UserId))
            {
                return Result.Fail(new ValidationError("UserId must be specified"));
            }

            if (IsNullOrWhiteSpace(request.Description))
            {
                return Result.Fail(new ValidationError("Description must be specified"));
            }

            if (IsNullOrWhiteSpace(request.Question))
            {
                return Result.Fail(new ValidationError("Question must be specified"));
            }

            if (IsNullOrWhiteSpace(request.Title))
            {
                return Result.Fail(new ValidationError("Title must be specified"));
            }

            if (request.Latitude is null)
            {
                return Result.Fail(new ValidationError("Latitude must be specified"));
            }

            if (request.Longitude is null)
            {
                return Result.Fail(new ValidationError("Longitude must be specified"));
            }

            if (request.Image is null)
            {
                return Result.Fail(new ValidationError("Image is mandatory"));
            }

            var entity = PostEntity.Create(request.Title, request.Question, request.Description, (float) request.Latitude, (float) request.Longitude, request.UserId);

            try
            {
                await _repo.AddItemAsync(entity);
                var fileName = $"{Guid.NewGuid()}-{request.Image.FileName}";
                await _storageService.UploadImageAsync(request.Image, fileName);
                var imageUrl = $"{_configuration["ImageStorageServiceUri"]}/{fileName}";
                entity.SetImageUrl(imageUrl);
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
