using System;
using System.IO;
using Appropose.Core.Interfaces;
using Appropose.Infrastructure.AppSettings;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Appropose.Infrastructure.CosmosDbData.Repository;
using Appropose.Infrastructure.Extensions;
using DevOne.Security.Cryptography.BCrypt;

[assembly: FunctionsStartup(typeof(Appropose.Functions.Startup))]

namespace Appropose.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Configurations
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            // Use a singleton Configuration throughout the application
            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging();

            services.AddMediatR(typeof(Startup));

            services.Add

            // Bind database-related bindings
            CosmosDbSettings cosmosDbConfig = configuration.GetSection("ToDoListCosmosDb").Get<CosmosDbSettings>();
            // register CosmosDB client and data repositories
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                cosmosDbConfig.PrimaryKey,
                cosmosDbConfig.DatabaseName,
                cosmosDbConfig.Containers);

            services.SetupStorage(configuration);

            services.AddScoped<IPostRepository, PostRepository>();
        }
    }
}