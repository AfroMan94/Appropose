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
    public class GetAllPostsQuery : IRequest<Result<IEnumerable<GetAllPostsQueryResponse>>>
    {
    }

    public class GetAllPostsQueryResponse
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Question { get; set; }
        public string ImageUrl { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public int AngryCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string[] Solutions { get; set; }
        public string Retailer { get; set; }
        public string RetailerAddress { get; set; }
    }

    public class GetAllPostsQueryHandler : IRequestHandler<GetAllPostsQuery, Result<IEnumerable<GetAllPostsQueryResponse>>>
    {
        private readonly ILogger _logger;
        private readonly IPostRepository _repo;
        private readonly IMapper _mapper;


        public GetAllPostsQueryHandler(ILogger logger, IPostRepository repo, IMapper mapper)
        {
            _logger = logger;
            _repo = repo;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<GetAllPostsQueryResponse>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var posts = await _repo.GetAllPostsAsync();
                return Result.Ok(_mapper.Map<IEnumerable<GetAllPostsQueryResponse>>(posts.OrderByDescending(post => post.CreatedOn).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong during fetching posts");
                return Result.Fail(new RuntimeError("Something went wrong during fetching posts"));
            }
        }
    }
}
