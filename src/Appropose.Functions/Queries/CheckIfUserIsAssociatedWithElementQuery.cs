using System.Threading;
using System.Threading.Tasks;
using Appropose.Core.Interfaces;
using Appropose.Functions.FluentErrors;
using FluentResults;
using MediatR;
using static System.String;

namespace Appropose.Functions.Queries
{
    public class CheckIfUserIsAssociatedWithElementQuery : IRequest<Result<bool>>
    {
        public string UserId { get; }
        public string ElementId { get; }

        public CheckIfUserIsAssociatedWithElementQuery(string userId, string elementId)
        {
            UserId = userId;
            ElementId = elementId;
        }
    }

    public class CheckIfUserIsAssociatedWithElementQueryHandler : IRequestHandler<CheckIfUserIsAssociatedWithElementQuery, Result<bool>>
    {
        private readonly IUserElementRepository _repository;

        public CheckIfUserIsAssociatedWithElementQueryHandler(IUserElementRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<bool>> Handle(CheckIfUserIsAssociatedWithElementQuery request, CancellationToken cancellationToken)
        {
            if (IsNullOrWhiteSpace(request.UserId))
            {
                return Result.Fail(new ValidationError("UserId is mandatory"));
            }

            if (IsNullOrWhiteSpace(request.ElementId))
            {
                return Result.Fail(new ValidationError("ElementId is mandatory"));
            }

            var entity = await _repository.GetUserAssociationAsync(request.UserId, request.ElementId);

            return Result.Ok(entity != null);
        }
    }
}
