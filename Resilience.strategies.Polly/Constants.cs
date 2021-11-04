namespace Resilience.strategies.Polly
{
    public static class Constants
    {
        public static class PolicyName
        {
            public const string CircuitBreaker = nameof(CircuitBreaker);
            public const string FallBack = nameof(FallBack);
            public const string Retry = nameof(Retry);
        }
    }
}
