using Appropose.Core.Domain.Entities;
using System.Threading.Tasks;

namespace Appropose.Core.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        public Task<UserEntity> GetUserByLogin(string login);
    }
}
