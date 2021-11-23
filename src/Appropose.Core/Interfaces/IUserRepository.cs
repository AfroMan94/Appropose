using Appropose.Core.Domain.Entities;

namespace Appropose.Core.Interfaces
{
    /*
        * authentication method
        * azure function for authentication
        * mediatr command and handler
     */
    public interface IUserRepository : IRepository<UserEntity>
    {
    }
}
