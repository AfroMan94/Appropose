using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;

namespace Appropose.Core.Interfaces
{
    public interface IUserElementRepository : IRepository<UserElementEntity>
    {
        Task<UserElementEntity> GetUserAssociationAsync(string userId, string elementId);
    }
}
