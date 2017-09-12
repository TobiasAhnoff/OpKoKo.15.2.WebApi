using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpKokoDemo.Config;
using OpKokoDemo.Requests;

namespace OpKokoDemo.Services
{
    public class PingService : IPingService
    {
        private readonly PingServiceConfig _config;
        private readonly IHttpServiceClient _httpServiceClient;
        private ILogger _logger;
        public PingService(IHttpServiceClient httpServiceClient, IOptions<PingServiceConfig> config)
        {
            if (config == null || config.Value == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _config = config.Value;
            _httpServiceClient = httpServiceClient;
            //_logger = logger;
        }
        public async Task<Tuple<int, long>> Execute(ExecuteServiceRequest request)
        {
            var counter = 0;
            Tuple<HttpResponseMessage, long> response;
            do
            {
                response = await _httpServiceClient.GetAsync(request.Uri);
            }
            while (!response.Item1.IsSuccessStatusCode && ++counter < _config.Retries);

            if (counter >= _config.Retries)
            {
                throw new ApplicationException($"Request to {request.Uri} with {_config.Retries} retries failed");
            }

            return new Tuple<int, long>(counter, response.Item2);
        }

    }
}
