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
        /// <summary>
        ///     CosmosDB container name
        /// </summary>
        public override string ContainerName { get; } = "Posts";

        /// <summary>
        ///     Returns the value of the partition key
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public PostRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        public async Task<IEnumerable<PostEntity>> GetAllPostsAsync()
        {
            string query = @"SELECT * FROM posts";
            var result = await GetItemsAsync(query);
            return result.ToList();
        }
    }
}
