using System.Collections.Generic;
using System.Net.Http;
using Polly;
using Polly.Wrap;
using Resilience.strategies.Polly.Abstracts;
using Resilience.strategies.Polly.Configuration;

namespace Resilience.strategies.Polly
{
    public sealed class ResilienceStrategyBuilder : IResilienceStrategyBuilder
    {
        private readonly List<IAsyncPolicy<HttpResponseMessage>> _asyncPolicies;
        private readonly IResiliencePolicyBuilder _resiliencePolicyBuilder;

        public ResilienceStrategyBuilder(IResiliencePolicyBuilder resiliencePolicyBuilder)
        {
            _asyncPolicies = new List<IAsyncPolicy<HttpResponseMessage>>();
            _resiliencePolicyBuilder = resiliencePolicyBuilder;
        }

        public IFallBackBuilder Instance => this;

        public IPolicyWrapBuilder AddCircuitBreakerPolicy(CircuitBreakerOptions options)
        {
            if (options is null) return this;

            var policies = _resiliencePolicyBuilder.GetCircuitBreakerPolicies(options);
            _asyncPolicies.AddRange(policies);
            return this;
        }

        public ICircuitBreakerBuilder AddWaitAndRetryPolicy(RetryOptions options)
        {
            if (options is null || !options.Active) return this;

            var policy = _resiliencePolicyBuilder.GetRetryPolicy(options);
            _asyncPolicies.Add(policy);
            return this;
        }

        public IWaitAndRetryBuilder AddFallBackPolicy()
        {
            var policy = _resiliencePolicyBuilder.GetFallBackPolicy();
            _asyncPolicies.Add(policy);
            return this;
        }

        public AsyncPolicyWrap<HttpResponseMessage> Build()
        {
            return Policy.WrapAsync(_asyncPolicies.ToArray());
        }
    }
}