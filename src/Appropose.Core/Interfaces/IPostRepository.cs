using System.Collections.Generic;
using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;

namespace Appropose.Core.Interfaces
{
    public interface IPostRepository : IRepository<PostEntity>
    {
        Task<IEnumerable<PostEntity>> GetAllPostsAsync();
        Task<IEnumerable<PostEntity>> GetAllUserPostsAsync(string userId);
    }
}
