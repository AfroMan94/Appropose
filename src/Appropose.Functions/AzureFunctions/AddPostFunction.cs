using System.Globalization;
using System.Threading.Tasks;
using Appropose.Functions.Commands;
using Appropose.Functions.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;

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
            if (!formData.TryGetValue("title", out var titleValue) ||
                !formData.TryGetValue("description", out var descriptionValue) ||
                !formData.TryGetValue("latitude", out var latitudeValue) ||
                !formData.TryGetValue("longitude", out var longtitudeValue)
               )
            {
                return new BadRequestObjectResult("Not all required fields are specified!");
            }

            float latitude;
            float longtitude;
            if (!float.TryParse(latitudeValue.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out latitude) ||
                !float.TryParse(longtitudeValue.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out longtitude))
            {
                return new BadRequestObjectResult("Latitude or Longtitude format is wrong!");
            }

            var command = new AddPostCommand(
                titleValue, 
                descriptionValue, 
                latitude, 
                longtitude,
                imageFile);
            
            var result = await _mediator.Send(command);
            
            return result.IsFailed ? result.GetErrorResponse() : new OkObjectResult("Post successfully created.");
        }
    }
}
