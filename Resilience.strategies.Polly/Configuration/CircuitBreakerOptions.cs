namespace Resilience.strategies.Polly.Configuration
{
    public sealed class CircuitBreakerOptions
    {
        public int ExceptionsAllowedBeforeBreaking { get; set; }
        public int DurationOfBreak { get; set; }
    }
}