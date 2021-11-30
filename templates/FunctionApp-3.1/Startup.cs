using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using FunctionApp.Http;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureServices(builder.Services);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // retrieve IConfiguration instance if necessary
            // to configure the Function App

            var configuration = services.GetConfiguration();

            // this is an example of how to best use
            // a typed HTTP client using the Refit library

            services.UseHttpBinOrgApi();
        }
    }

    public static class IServiceCollectionConfigurationExtensions
    {
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            return services.BuildServiceProvider().GetService<IConfiguration>();
        }
    }
}
