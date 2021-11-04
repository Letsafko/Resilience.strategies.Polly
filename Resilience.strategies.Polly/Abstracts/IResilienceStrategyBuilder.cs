namespace Resilience.strategies.Polly.Abstracts
{
    public interface IResilienceStrategyBuilder : IPolicyWrapBuilder,
        ICircuitBreakerBuilder,
        IWaitAndRetryBuilder,
        IFallBackBuilder
    {
        IFallBackBuilder Instance { get; }
    }
}
