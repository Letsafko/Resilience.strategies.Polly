namespace Resilience.strategies.Polly.Configuration
{
    public sealed class Fault
    {
        public string Content { get; set; }
        public int StatusCode { get; set; }
    }
}