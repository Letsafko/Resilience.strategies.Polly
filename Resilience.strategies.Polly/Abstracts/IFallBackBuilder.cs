namespace Resilience.strategies.Polly.Abstracts
{
    public interface IFallBackBuilder
    {
        IWaitAndRetryBuilder AddFallBackPolicy();
    }
}
