using Microsoft.Azure.Cosmos;

namespace Appropose.Infrastructure.CosmosDbData.Interfaces
{
    public interface ICosmosDbContainer
    {
        Container _container { get; }
    }
}
