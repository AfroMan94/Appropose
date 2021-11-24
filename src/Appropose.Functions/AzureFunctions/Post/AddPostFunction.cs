using System.Globalization;
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
    public class AddPostFunction
    {
        private readonly IMediator _mediator;

        public AddPostFunction(IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("AddPostFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            var formData = await req.ReadFormAsync();
            log.LogInformation("formData", formData);
            var imageFile = req.Form.Files?["image"];
            if (!formData.TryGetValue("title", out var titleValue) ||
                !formData.TryGetValue("description", out var descriptionValue) ||
                !formData.TryGetValue("question", out var questionValue) ||
                !formData.TryGetValue("latitude", out var latitudeValue) ||
                !formData.TryGetValue("longitude", out var longtitudeValue) ||
                !formData.TryGetValue("userId", out var userIdValue)
               )
            {
                return new BadRequestObjectResult("Not all required fields are specified!");
            }

            if (!float.TryParse(latitudeValue.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var latitude) ||
                !float.TryParse(longtitudeValue.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var longtitude))
            {
                return new BadRequestObjectResult("Latitude or Longtitude format is wrong!");
            }

            var command = new AddPostCommand(
                titleValue, 
                questionValue,
                descriptionValue,
                latitude, 
                longtitude,
                userIdValue,
                imageFile);
            
            var result = await _mediator.Send(command);
            
            return result.IsFailed ? result.GetErrorResponse() : new OkObjectResult("Post successfully created.");
        }
    }
}
