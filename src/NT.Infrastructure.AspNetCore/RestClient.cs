using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microphone;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace NT.Infrastructure.AspNetCore
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
            var serviceInstance = await SelectServiceInfo(serviceName);

            var uri = new Uri($"http://{serviceInstance.Host}:{serviceInstance.Port}{path}");
            _logger.LogInformation("[INFO] GET Uri:" + uri);

            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // _client.DefaultRequestHeaders.Add("Authorization", "Bearer [token]");

            var response = await _client.GetAsync(uri);
            if (!response.IsSuccessStatusCode) return await Task.FromResult(new TReturnMessage());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<TReturnMessage> PostAsync<TReturnMessage>(string serviceName, string path, object dataObject = null)
            where TReturnMessage : class, new()
        {
            var serviceInstance = await SelectServiceInfo(serviceName);

            var uri = new Uri($"http://{serviceInstance.Host}:{serviceInstance.Port}{path}");
            _logger.LogInformation("[INFO] POST Uri:" + uri);

            var content = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";

            var response = await _client.PostAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return await Task.FromResult(new TReturnMessage());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<TReturnMessage> PutAsync<TReturnMessage>(string serviceName, string path, object dataObject = null)
            where TReturnMessage : class, new()
        {
            var serviceInstance = await SelectServiceInfo(serviceName);

            var uri = new Uri($"http://{serviceInstance.Host}:{serviceInstance.Port}{path}");
            _logger.LogInformation("[INFO] PUT Uri:" + uri);

            var content = dataObject != null ? JsonConvert.SerializeObject(dataObject) : "{}";

            var response = await _client.PutAsync(uri, new StringContent(content, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return await Task.FromResult(new TReturnMessage());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TReturnMessage>(result);
        }

        public async Task<bool> DeleteAsync(string serviceName, string path)
        {
            var serviceInstance = await SelectServiceInfo(serviceName);

            var uri = new Uri($"http://{serviceInstance.Host}:{serviceInstance.Port}{path}");
            _logger.LogInformation("[INFO] DELETE Uri:" + uri);

            var response = await _client.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();

            if (!response.IsSuccessStatusCode) return await Task.FromResult(false);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(result);
        }

        private static async Task<ServiceInformation> SelectServiceInfo(string serviceName)
        {
            var serviceInstances = await Cluster.Client.GetServiceInstancesAsync(serviceName);

            // TODO: should do the load balance technic
            return serviceInstances.FirstOrDefault();
        }
    }
}