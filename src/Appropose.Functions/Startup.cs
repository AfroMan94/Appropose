using System.IO;
using Appropose.Core.Interfaces;
using Appropose.Functions.MappingProfiles;
using Appropose.Infrastructure.AppSettings;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Appropose.Infrastructure.CosmosDbData.Repository;
using Appropose.Infrastructure.Extensions;
using Appropose.Infrastructure.Services;
using AutoMapper;

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
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddLogging();

            services.AddMediatR(typeof(Startup));

            CosmosDbSettings cosmosDbConfig = configuration.GetSection("ApproposeCosmosDb").Get<CosmosDbSettings>();
            services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                cosmosDbConfig.PrimaryKey,
                cosmosDbConfig.DatabaseName,
                cosmosDbConfig.Containers);

            services.AddScoped<IStorageService, AzureBlobStorageService>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISolutionRepository, SolutionRepository>();
            services.AddScoped<IUserElementRepository, UserElementRepository>();

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            var mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}