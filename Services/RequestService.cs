using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Services
{
    public class RequestService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RequestService> _logger;

        public RequestService(IHttpClientFactory httpClientFactory, ILogger<RequestService> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> MakeRequestAsync(string requestUri, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(requestUri))
                throw new ArgumentException("Request URI cannot be null or whitespace.", nameof(requestUri));

            var httpClient = _httpClientFactory.CreateClient();

            try
            {
                var response = await httpClient.GetAsync(requestUri, cancellationToken);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "HttpRequestException caught while making a request to {RequestUri}", requestUri);
                // keeps stack trace
                throw;
            }
        }
    }
}
