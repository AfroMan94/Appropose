using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using MediatR;
using Appropose.Functions.Commands;
using ToDoList.Functions.Extensions;

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
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            UserLoginCommand command = JsonConvert.DeserializeObject<UserLoginCommand>(requestBody);


            var result = await _mediator.Send(command);
            if (result.IsFailed)
            {
                return result.GetErrorResponse();
            }

            return new OkObjectResult(result.ValueOrDefault);
        }
    }
}
