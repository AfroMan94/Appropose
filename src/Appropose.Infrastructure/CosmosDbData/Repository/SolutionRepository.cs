using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;
using Appropose.Core.Interfaces;
using Appropose.Infrastructure.CosmosDbData.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Appropose.Infrastructure.CosmosDbData.Repository
{
    public class SolutionRepository : CosmosDbRepository<SolutionEntity>, ISolutionRepository
    {
        public SolutionRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory)
        {
        }

        public override string ContainerName => "Solutions";
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public async Task<IEnumerable<SolutionEntity>> GetSolutionsForPostAsync(string postId)
        {
            string query = @"SELECT * FROM solutions WHERE postId = @postId";
            var queryParams = new Dictionary<string, object> {{ "@postId", postId }};
            return await GetItemsAsync(query, queryParams);
        }
    }
}
