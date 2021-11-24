using Appropose.Core.Domain.Entities;
using Appropose.Core.Interfaces;
using Appropose.Infrastructure.CosmosDbData.Interfaces;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Appropose.Infrastructure.CosmosDbData.Repository
{
    public class UserRepository : CosmosDbRepository<UserEntity>, IUserRepository
    {
        public UserRepository(ICosmosDbContainerFactory factory) : base(factory)
        { }

        public override string ContainerName => "Users";
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId);

        public async Task<UserEntity> GetUserByLogin(string login)
        {
            string query = @"SELECT * FROM users u WHERE u.login = @login";
            var queryParams = new Dictionary<string, object> {{ "@login", login }};
            return await GetItemAsync(query, queryParams);
        }
    }
}
