using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using FunctionApp.Interop;
using System.Threading;

namespace FunctionApp
{
    public class Functions
    {
        private readonly IConfiguration configuration_;
        private readonly IHttpBinOrgApi client_;

        public Functions(IConfiguration configuration, IHttpBinOrgApi client)
        {
            configuration_ = configuration;
            client_ = client;
        }

        [FunctionName("Functions")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [Blob("container", FileAccess.Read, Connection = "AzureWebJobsStorage")] CloudBlobContainer container,
            ILogger log,
            CancellationToken cancellationToken)
        {
            using var cancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, req.HttpContext.RequestAborted);

            log.LogInformation("C# HTTP trigger function processed a request.");

            // retrieve client tracking identifier
            // from request headers

            var trackingId = Guid.NewGuid().ToString("d");
            var blob = container.GetBlockBlobReference(trackingId);
            using (var stream = await blob.OpenWriteAsync())
            using (var writer = new StreamWriter(stream))
                writer.WriteLine("Hello, world!");

            // transmit correlation/tracking identifier

            await client_.StatusCodes(201);

            await client_.GetRequestHeaders();

            // handle incoming request

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}
