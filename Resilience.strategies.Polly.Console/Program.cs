using Microsoft.Extensions.DependencyInjection;
using Resilience.strategies.Polly.Abstracts;
using System;

namespace Resilience.strategies.Polly.Startup
{
    internal class Program
    {
        private const string ApiUrl = "http://localhost:2562";

        public static void Main(string[] args)
        {
            var services = new ServiceCollection();
            Console.WriteLine("Hello World!");
        }

        private static IServiceCollection Configure()
        {
            var services = new ServiceCollection();
            services
                .AddSingleton<IResiliencePolicyBuilder, ResiliencePolicyBuilder>()
                .AddSingleton<IResilienceStrategyBuilder, ResilienceStrategyBuilder>()
                .AddHttpClient<IDummyApiClient, DummyApiClient>(nameof(DummyApiClient),
                    client => { client.BaseAddress = new Uri(ApiUrl); });

            return services;
        }
    }
}