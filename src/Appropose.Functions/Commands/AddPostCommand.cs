using System;
using System.Threading;
using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;
using Appropose.Core.Interfaces;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ToDoList.Functions.FluentErrors;
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
        private readonly IStorageService _storage;

        public AddPostCommandHandler(ILogger logger, IPostRepository repo, IStorageService storage)
        {
            _logger = logger;
            _repo = repo;
            _storage = storage;
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
                var filePath = await _storage.UploadFileAsync(request.Image, request.Image.FileName);
                entity.UpdateImagePath(filePath);
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
