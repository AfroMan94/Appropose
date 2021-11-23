using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Appropose.Core.Interfaces;
using Appropose.Functions.FluentErrors;
using AutoMapper;
using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Appropose.Functions.Queries
{
    public class GetSolutionsForPostQuery : IRequest<Result<IEnumerable<GetSolutionForPostQueryResponse>>>
    {
        public string PostId { get; }
        public GetSolutionsForPostQuery(string postId) => PostId = postId;
    }

    public class GetSolutionForPostQueryResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string PostId { get; set; }
        public string UserId { get; set; }
    }

    public class GetSolutionsForPostQueryHandler : IRequestHandler<GetSolutionsForPostQuery, Result<IEnumerable<GetSolutionForPostQueryResponse>>>
    {
        private readonly ILogger _logger;
        private readonly ISolutionRepository _solutionRepository;
        private readonly IMapper _mapper;


        public GetSolutionsForPostQueryHandler(ILogger logger, ISolutionRepository solutionRepository, IMapper mapper)
        {
            _logger = logger;
            _solutionRepository = solutionRepository;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<GetSolutionForPostQueryResponse>>> Handle(GetSolutionsForPostQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var solutions = await _solutionRepository.GetSolutionsForPostAsync(request.PostId);
                return Result.Ok(_mapper.Map<IEnumerable<GetSolutionForPostQueryResponse>>(solutions.OrderByDescending(solution => solution.CreatedOn).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong during fetching solutions");
                return Result.Fail(new RuntimeError("Something went wrong during fetching solutions"));
            }
        }
    }
}
