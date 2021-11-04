using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Resilience.strategies.Polly.Abstracts;
using Resilience.strategies.Polly.Extensions;
using Resilience.strategies.Polly.Test.MessageHandler;

namespace Resilience.strategies.Polly.Test.Builder
{
    internal sealed class ServiceCollectionBuilder
    {
        private readonly IResiliencePolicyBuilder _resiliencePolicyBuilder;
        private readonly IResilienceStrategyBuilder _resilienceStrategyBuilder;
        internal readonly ILogger<ResiliencePolicyBuilder> Logger;
        internal IConfiguration Configuration;

        private ServiceCollectionBuilder()
        {
            Logger = new Mock<ILogger<ResiliencePolicyBuilder>>().Object;
            _resiliencePolicyBuilder = new ResiliencePolicyBuilder(Logger);
            _resilienceStrategyBuilder = new ResilienceStrategyBuilder(_resiliencePolicyBuilder);
            Configuration =
                ConfigurationBuilder
                    .Instance
                    .WithJsonFile("Files/appSettings.json")
                    .Build();
        }

        internal static ServiceCollectionBuilder Instance => new();

        internal ServiceCollectionBuilder WithConfiguration(string appSettingFilePath)
        {
            Configuration =
                ConfigurationBuilder
                    .Instance
                    .WithJsonFile(appSettingFilePath)
                    .Build();

            return this;
        }

        public IServiceCollection Build()
        {
            var services = new ServiceCollection();
            services
                .AddSingleton(_resiliencePolicyBuilder)
                .AddSingleton(_resilienceStrategyBuilder);

            return services
                .AddHttpClient<IDummyApiClient, DummyApiClient>(nameof(DummyApiClient),
                    client => { client.BaseAddress = new Uri(ApiUrl); })
                .AddResilienceStrategy(Configuration, ServiceToTest)
                .AddHttpMessageHandler(() => new DummyMessageHandler())
                .Services;
        }

        #region ctor & fields

        private const string ServiceToTest = "Polly:ServiceClient";
        private const string ApiUrl = "http://localhost:2829";

        #endregion
    }
}