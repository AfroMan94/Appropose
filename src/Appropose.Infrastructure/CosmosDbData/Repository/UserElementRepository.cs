using System.Collections.Generic;
using System.Threading.Tasks;
using Appropose.Core.Domain.Entities;
using Appropose.Core.Interfaces;
using Appropose.Infrastructure.CosmosDbData.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Appropose.Infrastructure.CosmosDbData.Repository
{
    public class UserElementRepository : CosmosDbRepository<UserElementEntity>, IUserElementRepository
    {
        public UserElementRepository(ICosmosDbContainerFactory cosmosDbContainerFactory) : base(cosmosDbContainerFactory)
        {
        }

        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public override string ContainerName => "UserElements";

        public async Task<UserElementEntity> GetUserAssociationAsync(string userId, string elementId)
        {
            var query = @"SELECT * FROM userElements ue WHERE ue.userId = @userId and ue.elementId = @elementId";
            var queryParams = new Dictionary<string, object> {{ "@userId", userId }, { "@elementId", elementId }};
            return await GetItemAsync(query, queryParams);
        }

        public async Task<IEnumerable<UserElementEntity>> GetPostAssociationsAsync(string elementId)
        {
            var query = @"SELECT * FROM userElements ue WHERE ue.elementId = @elementId";
            var queryParams = new Dictionary<string, object> {{ "@elementId", elementId }};
            return await GetItemsAsync(query, queryParams);
        }
    }
}
