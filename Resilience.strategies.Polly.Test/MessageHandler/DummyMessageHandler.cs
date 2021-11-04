using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Resilience.strategies.Polly.Test.MessageHandler
{
    internal sealed class DummyMessageHandler : DelegatingHandler
    {
        private static int _counter;

        public DummyMessageHandler()
        {
            _counter = 0;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response;
            if (_counter == 3)
            {
                response = new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                _counter++;
                response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            return await Task.FromResult(response);
        }
    }
}