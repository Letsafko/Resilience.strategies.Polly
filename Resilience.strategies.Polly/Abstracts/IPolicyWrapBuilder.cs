using Polly.Wrap;
using System.Net.Http;

namespace Resilience.strategies.Polly.Abstracts
{
    public interface IPolicyWrapBuilder
    {
        AsyncPolicyWrap<HttpResponseMessage> Build();
    }
}
