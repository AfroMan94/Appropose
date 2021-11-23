using FluentResults;

namespace Appropose.Functions.FluentErrors
{
    public class ValidationError : Error
    {
        public ValidationError(string message) : base(message)
        {
        }
    }
}
