using System.IO;
using System.Threading.Tasks;
using Appropose.Functions.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Appropose.Functions.Extensions;

namespace Appropose.Functions.AzureFunctions.Post
{
    public class ToggleUserAssociationWithElementFunction
    {
        private readonly IMediator _mediator;

        public ToggleUserAssociationWithElementFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("ToggleUserAssociationWithElementFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var command = JsonConvert.DeserializeObject<ToggleUserAssociationWithElementCommand>(requestBody);
            var result = await _mediator.Send(command);
            return result.IsFailed ? result.GetErrorResponse() : new OkObjectResult("Association successfully toggled");
        }
    }
}
