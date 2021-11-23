using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Appropose.Infrastructure.AppSettings;
using Appropose.Infrastructure.CosmosDbData;
using Appropose.Infrastructure.CosmosDbData.Interfaces;
using Microsoft.Azure.Cosmos;

namespace Appropose.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCosmosDb(this IServiceCollection services,
                                                     string endpointUrl,
                                                     string primaryKey,
                                                     string databaseName,
                                                     List<ContainerInfo> containers)
        {
            var client = new CosmosClient(endpointUrl, primaryKey);
            var cosmosDbClientFactory = new CosmosDbContainerFactory(client, databaseName, containers);

            cosmosDbClientFactory.EnsureDbSetupAsync().Wait();

            services.AddSingleton<ICosmosDbContainerFactory>(cosmosDbClientFactory);

            return services;
        }
    }
}
