using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Polly;
using Resilience.strategies.Polly.Abstracts;
using Resilience.strategies.Polly.Configuration;

namespace Resilience.strategies.Polly.Extensions
{
    public static class ResilienceStrategyExtensions
    {
        public static IHttpClientBuilder AddResilienceStrategy(this IHttpClientBuilder httpClientBuilder,
            IConfiguration configuration,
            string serviceScheme)
        {
            var pollyOptions = GetPolicySettings(configuration, serviceScheme);
            if (!pollyOptions.Active) return httpClientBuilder;

            var policyWrap = httpClientBuilder
                .Services
                .BuildServiceProvider()
                .GetRequiredService<IResilienceStrategyBuilder>()
                .Instance
                .AddFallBackPolicy()
                .AddWaitAndRetryPolicy(pollyOptions.RetryOptions)
                .AddCircuitBreakerPolicy(pollyOptions.CircuitBreakerOptions)
                .Build();

            return AddPolicyHandler(httpClientBuilder, policyWrap);
        }

        #region private helpers

        private static IHttpClientBuilder AddPolicyHandler(IHttpClientBuilder builder,
            IAsyncPolicy<HttpResponseMessage> policyWrap)
        {
            void Action(HttpClientFactoryOptions options)
            {
                options
                    .HttpMessageHandlerBuilderActions
                    .Add(a => a.AdditionalHandlers.Add(new PolicyHttpMessageHandler(policyWrap)));
            }

            builder
                .Services
                .Configure<HttpClientFactoryOptions>(builder.Name, Action);

            return builder;
        }

        private static PollyOptions GetPolicySettings(IConfiguration configuration,
            string serviceScheme)
        {
            if (string.IsNullOrWhiteSpace(serviceScheme)) throw new ArgumentNullException(nameof(serviceScheme));

            return
                configuration
                    .GetSection(serviceScheme)
                    .Get<PollyOptions>()
                ?? throw new InvalidOperationException($"unable to retrieve resilience options {nameof(PollyOptions)}");
        }

        #endregion
    }
}