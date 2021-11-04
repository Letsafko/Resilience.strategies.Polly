using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resilience.strategies.Polly.Startup
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
            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.Accepted));
        }
    }
}