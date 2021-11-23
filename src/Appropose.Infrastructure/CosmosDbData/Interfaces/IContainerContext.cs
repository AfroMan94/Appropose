using Appropose.Core.Domain.Entities;
using Microsoft.Azure.Cosmos;

namespace Appropose.Infrastructure.CosmosDbData.Interfaces
{
    public interface IContainerContext<T> where T : BaseEntity
    {
        string ContainerName { get; }
        PartitionKey ResolvePartitionKey(string entityId);
    }
}
