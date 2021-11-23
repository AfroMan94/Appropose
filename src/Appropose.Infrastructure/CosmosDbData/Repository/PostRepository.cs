using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;
using Appropose.Core.Interfaces;
using Appropose.Infrastructure.CosmosDbData.Interfaces;

namespace Appropose.Infrastructure.CosmosDbData.Repository
{
    public class PostRepository : CosmosDbRepository<PostEntity>, IPostRepository
    {
        public override string ContainerName => "Posts";

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public PostRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        public async Task<IEnumerable<PostEntity>> GetAllPostsAsync()
        {
            string query = @"SELECT * FROM posts";
            var result = await GetItemsAsync(query);
            return result.ToList();
        }

        public async Task<IEnumerable<PostEntity>> GetAllUserPostsAsync(string userId)
        {
            string query = @"SELECT * FROM posts WHERE userId = @userId";
            var queryParams = new Dictionary<string, object> {{ "@userId", userId }};
            return await GetItemsAsync(query, queryParams);
        }
    }
}
