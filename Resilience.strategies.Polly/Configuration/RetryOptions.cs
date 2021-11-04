namespace Resilience.strategies.Polly.Configuration
{
    public sealed class RetryOptions
    {
        public int MaxRetryAttempts { get; set; }
        public Fault[] RetryFaults { get; set; }
        public bool Active { get; set; }
    }
}