using System.Threading.Tasks;
using Appropose.Functions.Extensions;
using Appropose.Functions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace Appropose.Functions.AzureFunctions
{
    public class GetSolutionsForPostFunction
    {
        private readonly IMediator _mediator;

        public GetSolutionsForPostFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("GetSolutionsForPostFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            var query = new GetSolutionsForPostQuery(req.Query["postId"]);
            var result = await _mediator.Send(query);
            return result.IsFailed ? result.GetErrorResponse() : new OkObjectResult(result.Value);
        }
    }
}
