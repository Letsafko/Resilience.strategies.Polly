using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace Resilience.strategies.Polly.Test
{
    public interface IDummyApiClient
    {
        Task<HttpResponseMessage> SendAsync();
    }

    public sealed class DummyApiClient : IDummyApiClient
    {
        private readonly HttpClient _client;

        public DummyApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };

            request.SetPolicyExecutionContext(new Context());
            var response = await _client.SendAsync(request);

            response.RequestMessage = request;
            return response;
        }
    }
}