using System.Threading.Tasks;
using Appropose.Functions.Commands;
using Appropose.Functions.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Appropose.Functions.AzureFunctions
{
    public class AddSolutionFunction
    {
        private readonly IMediator _mediator;

        public AddSolutionFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("AddSolutionFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var formData = await req.ReadFormAsync();
            var imageFile = req.Form.Files["image"];
            if (!formData.TryGetValue("title", out var titleValue) ||
                !formData.TryGetValue("description", out var descriptionValue) ||
                !formData.TryGetValue("userId", out var userIdValue) ||
                !formData.TryGetValue("postId", out var postIdValue)
            )
            {
                return new BadRequestObjectResult("Not all required fields are specified!");
            }

            var command = new AddSolutionCommand(titleValue, descriptionValue, userIdValue, postIdValue, imageFile);

            var result = await _mediator.Send(command);

            return result.IsFailed ? result.GetErrorResponse() : new OkObjectResult("Solution successfully created.");
        }
    }
}
