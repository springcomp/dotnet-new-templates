using FunctionApp.Interop;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace FunctionApp.Http
{
    public static class HttpBinOrgServiceCollectionExtensions
    {
        private const string HttpBinOrgApiHost = "http://httpbin.org";

        public static IServiceCollection UseHttpBinOrgApi(
            this IServiceCollection services
            )
        {
            services.AddHttpClient("HttpBinOrgApi", (provider, client) =>
            {
                client.BaseAddress = new System.Uri(HttpBinOrgApiHost);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", "dotnet-core/3.1");
            })
                .AddTypedClient(c => RestService.For<IHttpBinOrgApi>(c))
                .AddHttpMessageHandler<LoggingHttpHandler>()
                ;

            services.AddTransient<LoggingHttpHandler>(serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<LoggingHttpHandler>>();
                return new LoggingHttpHandler(logger);
            });

            return services;
        }
    }
}