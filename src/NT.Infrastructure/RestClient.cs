using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microphone;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NT.Infrastructure
{
    public class RestClient
    {
        private readonly HttpClient _client;
        private readonly ILogger _logger;

        public RestClient(HttpClient client = null)
        {
            _client = client ?? new HttpClient();
            _logger = LogFactory.GetLogInstance<RestClient>();
        }

        public async Task<TReturnMessage> GetAsync<TReturnMessage>(string serviceName, string path)
            where TReturnMessage : class, new()
        {
            using (_client)
            {
                var serviceInstance = await SelectServiceInfo(serviceName);

                var uri = new Uri($"http://{serviceInstance.Host}:{serviceInstance.Port}{path}");
                _logger.LogInformation("[INFO] Uri:" + uri);

                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // _client.DefaultRequestHeaders.Add("Authorization", "Bearer [token]");

                var response = await _client.GetAsync(uri);
                if (!response.IsSuccessStatusCode) return await Task.FromResult(new TReturnMessage());
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TReturnMessage>(result);
            }
        }

        private static async Task<ServiceInformation> SelectServiceInfo(string serviceName)
        {
            var serviceInstances = await Cluster.Client.GetServiceInstancesAsync(serviceName);

            // TODO: should do the load balance technic
            return serviceInstances.FirstOrDefault();
        }
    }
}