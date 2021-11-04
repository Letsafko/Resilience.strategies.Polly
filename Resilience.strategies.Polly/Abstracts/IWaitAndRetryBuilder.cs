using Resilience.strategies.Polly.Configuration;

namespace Resilience.strategies.Polly.Abstracts
{
    public interface IWaitAndRetryBuilder
    {
        ICircuitBreakerBuilder AddWaitAndRetryPolicy(RetryOptions options);
    }
}
