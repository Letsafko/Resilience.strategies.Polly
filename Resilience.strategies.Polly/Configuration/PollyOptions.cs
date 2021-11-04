namespace Resilience.strategies.Polly.Configuration
{
    public sealed class PollyOptions
    {
        public CircuitBreakerOptions CircuitBreakerOptions { get; set; }
        public RetryOptions RetryOptions { get; set; }
        public bool Active { get; set; }
    }
}