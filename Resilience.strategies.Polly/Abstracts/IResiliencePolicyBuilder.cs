using Polly;
using Resilience.strategies.Polly.Configuration;
using System.Collections.Generic;
using System.Net.Http;

namespace Resilience.strategies.Polly.Abstracts
{
    public interface IResiliencePolicyBuilder
    {
        IEnumerable<IAsyncPolicy<HttpResponseMessage>> GetCircuitBreakerPolicies(CircuitBreakerOptions options);
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(RetryOptions options);
        IAsyncPolicy<HttpResponseMessage> GetFallBackPolicy();
    }
}
