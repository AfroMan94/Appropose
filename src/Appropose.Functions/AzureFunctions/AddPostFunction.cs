using System.Threading.Tasks;
using Appropose.Functions.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using ToDoList.Functions.Extensions;

namespace Appropose.Functions.AzureFunctions
{
    public class AddPostFunction
    {
        private readonly IMediator _mediator;

        public AddPostFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("AddPostFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {

            var formData = await req.ReadFormAsync();
            var imageFile = req.Form.Files["image"];

            var command = new AddPostCommand(formData["title"], formData["description"], formData["localization"], imageFile);
            
            var result = await _mediator.Send(command);
            
            if (result.IsFailed)
            {
                return result.GetErrorResponse();
            }

            return new OkObjectResult("Post successfully created.");
        }
    }
}
