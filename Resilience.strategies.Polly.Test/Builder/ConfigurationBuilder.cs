using System.IO;
using Microsoft.Extensions.Configuration;

namespace Resilience.strategies.Polly.Test.Builder
{
    internal sealed class ConfigurationBuilder
    {
        private readonly IConfigurationBuilder _builder;

        private ConfigurationBuilder()
        {
            _builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory());
        }

        internal static ConfigurationBuilder Instance => new();

        internal ConfigurationBuilder WithJsonFile(string filePath)
        {
            _builder.AddJsonFile(filePath);

            return this;
        }

        internal IConfiguration Build()
        {
            return _builder.Build();
        }
    }
}