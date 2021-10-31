using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    internal class LoggingHttpHandler : DelegatingHandler
    {
        private ILogger logger_;

        public LoggingHttpHandler(ILogger<LoggingHttpHandler> log)
        {
            logger_ = log;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LogHttpRequestStatus(request);
            LogHttpHeaders(request.Headers);
            LogHttpHeaders(request.Content?.Headers);
            LogEmptyLine();
            await LogHttpContent(request.Content);

            var response = await base.SendAsync(request, cancellationToken);

            LogHttpHeaders(response.Headers);
            LogHttpHeaders(response.Content?.Headers);
            LogEmptyLine();
            await LogHttpContent(response.Content);

            return response;
        }

        private void LogHttpRequestStatus(HttpRequestMessage request)
        {
            logger_.LogDebug($"{request.Method} {request.RequestUri} HTTP/1.1");
        }

        private void LogHttpHeaders(HttpHeaders headers)
        {
            if (headers == null)
                return;

            foreach (var header in headers)
                logger_.LogDebug($"{header.Key}: {String.Join(", ", header.Value)}");
        }

        public async Task LogHttpContent(HttpContent content)
        {
            if (content == null)
            {
                await Task.CompletedTask;
                return;
            }

            if (!logger_.IsEnabled(LogLevel.Debug))
                return;

        }

        private string GetHeader(HttpHeaders headers, string headerName)
        {
            if (headers.TryGetValues(headerName, out var _headerValue) && _headerValue.Count() > 0)
                return _headerValue.First();
            return null;
        }

        private void LogEmptyLine()
        {
            logger_.LogDebug("");
        }
    }
}