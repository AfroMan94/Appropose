using Appropose.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appropose.Core.Interfaces
{
    /*
        * authentication method
        * azure function for authentication
        * mediatr command and handler
     */
    public interface IUserRepository : IRepository<UserEntity>
    {
        public Task<UserEntity> GetUserByLogin(string login);
    }
}
