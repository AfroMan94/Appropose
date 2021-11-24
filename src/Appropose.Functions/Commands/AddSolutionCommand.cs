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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static System.String;

namespace Appropose.Functions.Commands
{
    public class AddSolutionCommand : IRequest<Result>
    {
        public string Title { get; }
        public string Description { get; }
        public string UserId { get; }
        public string PostId { get; }
        public IFormFile Image { get; }

        public AddSolutionCommand(string title, string description, string userId, string postId, IFormFile image)
        {
            Title = title;
            Description = description;
            UserId = userId;
            PostId = postId;
            Image = image;
        }
    }

    public class AddSolutionCommandHandler : IRequestHandler<AddSolutionCommand, Result>
    {
        private readonly ILogger _logger;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IConfiguration _configuration;
        private readonly IStorageService _storageService;

        public AddSolutionCommandHandler(ILogger logger, ISolutionRepository solutionRepository, IStorageService storageService, IConfiguration configuration)
        {
            _logger = logger;
            _solutionRepository = solutionRepository;
            _storageService = storageService;
            _configuration = configuration;
        }

        public async Task<Result> Handle(AddSolutionCommand request, CancellationToken cancellationToken)
        {
            if (!request.Image.IsImage())
            {
                return Result.Fail(new ValidationError("Image is not valid"));
            }

            if (IsNullOrWhiteSpace(request.UserId))
            {
                return Result.Fail(new ValidationError("UserId must be specified"));
            }

            if (IsNullOrWhiteSpace(request.PostId))
            {
                return Result.Fail(new ValidationError("PostId must be specified"));
            }

            if (IsNullOrWhiteSpace(request.Description))
            {
                return Result.Fail(new ValidationError("Description must be specified"));
            }

            if (IsNullOrWhiteSpace(request.Title))
            {
                return Result.Fail(new ValidationError("Title must be specified"));
            }

            if (request.Image is null)
            {
                return Result.Fail(new ValidationError("Image is mandatory"));
            }

            var entity = SolutionEntity.Create(request.Title, request.Description, request.UserId, request.PostId);

            try
            {
                await _solutionRepository.AddItemAsync(entity);
                var fileName = $"{Guid.NewGuid()}-{request.Image.FileName}";
                await _storageService.UploadImageAsync(request.Image, fileName);
                var imageUrl = $"{_configuration["ImageStorageServiceUri"]}/{fileName}";
                entity.SetImageUrl(imageUrl);
                await _solutionRepository.UpdateItemAsync(entity.Id, entity);
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
