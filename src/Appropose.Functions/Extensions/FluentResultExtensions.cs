using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using Appropose.Functions.FluentErrors;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Appropose.Functions.Extensions
{
    public static class FluentResultExtensions
    {
        public static IActionResult GetErrorResponse(this Result result)
            => BuildErrorResponse(result);

        public static ActionResult GetErrorResponse<T>(this Result<T> result)
            => BuildErrorResponse(result);

        private static ActionResult BuildErrorResponse(ResultBase result)
        {
            if (result.HasError<NotFoundError>())
            {
                var errors = result.Errors.OfType<NotFoundError>();
                return new NotFoundObjectResult(new { Message = GetErrorMessage(errors) });
            }

            if (result.HasError<ValidationError>())
            {
                var errors = result.Errors.OfType<ValidationError>();
                return new BadRequestObjectResult(new { Message = GetErrorMessage(errors) });
            }

            if (result.HasError<RuntimeError>())
            {
                var errors = result.Errors.OfType<RuntimeError>();
                return new ObjectResult(GetErrorMessage(errors) ) { StatusCode = 500 };
            }

            throw new InvalidOperationException("No errors to process.");
        }

        private static string GetErrorMessage(IEnumerable<Error> errors)
            => string.Join(Environment.NewLine, errors.Select(e => e.Message));
    }
}
