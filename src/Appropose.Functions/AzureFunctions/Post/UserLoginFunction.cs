using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using MediatR;
using Appropose.Functions.Commands;
using Appropose.Functions.Extensions;
using Microsoft.Extensions.Logging;

namespace Appropose.Functions.AzureFunctions
{
    public class UserLoginFunction
    {

        private readonly IMediator _mediator;

        public UserLoginFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("UserLoginFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger logger)
        {

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var command = JsonConvert.DeserializeObject<UserLoginCommand>(requestBody);
            logger.LogInformation("command", command);
            var result = await _mediator.Send(command);
            return result.IsFailed ? result.GetErrorResponse() : new OkObjectResult(result.ValueOrDefault);
        }
    }
}
