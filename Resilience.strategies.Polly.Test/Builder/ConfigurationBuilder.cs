using Microsoft.Extensions.Configuration;

namespace Resilience.strategies.Polly.Test.Builder
{
    internal sealed class ConfigurationBuilder
    {
        private readonly Microsoft.Extensions.Configuration.ConfigurationBuilder _builder;

        private ConfigurationBuilder()
        {
            _builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
        }

        internal static ConfigurationBuilder Instance => new();

        internal ConfigurationBuilder WithJsonFile(string filePath)
        {
            _builder
                .AddJsonFile(filePath);

            return this;
        }

        internal IConfiguration Build()
        {
            return _builder.Build();
        }
    }
}