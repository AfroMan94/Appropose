using FluentResults;

namespace Appropose.Functions.FluentErrors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string message) : base(message)
        {
        }
    }
}
