using System.Threading.Tasks;
using Appropose.Functions.Extensions;
using Appropose.Functions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

namespace Appropose.Functions.AzureFunctions.Get
{
    public class CheckIfUserIsAssociatedWithElementFunction
    {
        private readonly IMediator _mediator;

        public CheckIfUserIsAssociatedWithElementFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("CheckIfUserIsAssociatedWithElementFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {

            string userId = req.Query["userId"];
            string elementId = req.Query["elementId"];

            var query = new CheckIfUserIsAssociatedWithElementQuery(userId, elementId);
            var result = await _mediator.Send(query);
            return result.IsFailed ? result.GetErrorResponse() : new OkObjectResult(result.Value);
        }
    }
}
