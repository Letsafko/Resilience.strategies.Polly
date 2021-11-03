using Resilience.strategies.Polly.Configuration;

namespace Resilience.strategies.Polly.Abstracts
{
    public interface ICircuitBreakerBuilder
    {
        IPolicyWrapBuilder AddCircuitBreakerPolicy(CircuitBreakerOptions options);
    }
}
