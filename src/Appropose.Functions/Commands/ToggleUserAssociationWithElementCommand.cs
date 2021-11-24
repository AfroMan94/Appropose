using System.Threading;
using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;
using Appropose.Core.Interfaces;
using Appropose.Functions.FluentErrors;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using static System.String;

namespace Appropose.Functions.Commands
{
    public static class ElementType
    {
        public const string 
            Post = "post",
            Solution = "solution";
    }
    public class ToggleUserAssociationWithElementCommand : IRequest<Result>
    {
        public string UserId { get; }
        public string ElementId { get; }
        public string Type { get; }

        public ToggleUserAssociationWithElementCommand(string userId, string elementId, string type)
        {
            UserId = userId;
            ElementId = elementId;
            Type = type;
        }
    }

    public class ToggleUserAssociationWithElementCommandHandler : IRequestHandler<ToggleUserAssociationWithElementCommand, Result>
    {
        private readonly ILogger _logger;
        private readonly IUserElementRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IPostRepository _postRepository;

        public ToggleUserAssociationWithElementCommandHandler(ILogger logger, IUserElementRepository repository, 
            IPostRepository postRepository, ISolutionRepository solutionRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _repository = repository;
            _userRepository = userRepository;
            _solutionRepository = solutionRepository;
            _postRepository = postRepository;
        }
        public async Task<Result> Handle(ToggleUserAssociationWithElementCommand request, CancellationToken cancellationToken)
        {
            if (IsNullOrWhiteSpace(request.UserId))
            {
                return Result.Fail(new ValidationError("UserId is mandatory."));
            }

            if (IsNullOrWhiteSpace(request.ElementId))
            {
                return Result.Fail(new ValidationError("UserId is mandatory."));
            }

            if (IsNullOrWhiteSpace(request.Type))
            {
                return Result.Fail(new ValidationError("Type is mandatory."));
            }

            if (request.Type != ElementType.Post && request.Type != ElementType.Solution)
            {
                return Result.Fail(new ValidationError("Type is not recognized."));
            }

            var userEntity = await _userRepository.GetItemAsync(request.UserId);
            if (userEntity is null)
            {
                return Result.Fail(new NotFoundError("User not found"));
            }

            if (request.Type.ToLower() == ElementType.Post)
            {
                var elementEntity = await _postRepository.GetItemAsync(request.ElementId);
                if (elementEntity is null)
                {
                    return Result.Fail(new NotFoundError("Element not found"));
                }

            } else
            {
                var elementEntity = await _solutionRepository.GetItemAsync(request.ElementId);
                if (elementEntity is null)
                {
                    return Result.Fail(new NotFoundError("Element not found"));
                }
            }

            var association =
                await _repository.GetUserAssociationAsync(request.UserId, request.ElementId);

            if (association is null)
            {
                var entity = UserElementEntity.Create(request.UserId, request.ElementId);
                await _repository.AddItemAsync(entity);
            }
            else
            {
                await _repository.DeleteItemAsync(association.Id);
            }

            return Result.Ok();
        }
    }
}
