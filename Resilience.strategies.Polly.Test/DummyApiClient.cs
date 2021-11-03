using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resilience.strategies.Polly.Test
{
    public interface IDummyApiClient
    {
        Task<HttpResponseMessage> SendAsync();
    }

    public sealed class DummyApiClient : IDummyApiClient
    {
        private readonly HttpClient _client;

        private int _counter;

        public DummyApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> SendAsync()
        {
            if (_counter == 3)
                return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));

            _counter++;
            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
        }
    }
}