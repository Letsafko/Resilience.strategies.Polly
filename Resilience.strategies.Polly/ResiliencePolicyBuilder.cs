using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;
using Resilience.strategies.Polly.Abstracts;
using Resilience.strategies.Polly.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resilience.strategies.Polly
{
    public sealed class ResiliencePolicyBuilder : IResiliencePolicyBuilder
    {
        private readonly ILogger<ResiliencePolicyBuilder> _logger;
        public ResiliencePolicyBuilder(ILogger<ResiliencePolicyBuilder> logger)
        {
            _logger = logger;
        }

        public IEnumerable<IAsyncPolicy<HttpResponseMessage>> GetCircuitBreakerPolicies(CircuitBreakerOptions options)
        {
            return new List<IAsyncPolicy<HttpResponseMessage>>
            {
                Policy
                .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                .CircuitBreakerAsync(options.ExceptionsAllowedBeforeBreaking,
                    TimeSpan.FromSeconds(options.DurationOfBreak),
                    (@delegate, duration, context) =>
                    {
                        context[$"{context.PolicyKey}.OnBreak.DurationOfBreak"] = duration.TotalSeconds;
                        _logger.LogWarning(@delegate.Exception, $"policy: {context.PolicyKey} - durationOfBreak: {duration.Seconds} seconds.");
                    },
                    context =>
                    {
                        context[$"{context.PolicyKey}.OnReset"] = context;
                        _logger.LogWarning($"policy: {context.PolicyKey} - on reset");
                    },
                    () =>
                    {
                        _logger.LogWarning($"policy: {Constants.PolicyName.CircuitBreaker} - on half open.");
                    })
            };
        }

        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(RetryOptions options)
        {
            return
                HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(x => options.RetryFaults?.Any(t => t.StatusCode == (int)x.StatusCode) == true)
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    options.MaxRetryAttempts,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    onRetry: (@delegate, timespan, retries, context) =>
                    {
                        context[$"{context.PolicyKey}.onRetry.Attempts"] = retries;
                        _logger.LogWarning(@delegate.Exception, $"policy: {context.PolicyKey} - attempt n°: {retries}");
                    })
                .WithPolicyKey(Constants.PolicyName.Retry);

        }

        public IAsyncPolicy<HttpResponseMessage> GetFallBackPolicy()
        {
            return
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .Or<TimeoutRejectedException>()
                    .Or<BrokenCircuitException>()
                    .FallbackAsync
                    (
                        fallbackAction: (context, cancellationToken) =>
                         {
                             context[$"{context.PolicyKey}.FallBackAction"] = "called!";
                             return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                         },
                        onFallbackAsync: (@delegate, context) =>
                         {
                             context[$"{context.PolicyKey}.OnFallBack"] = "called!";
                             _logger.LogWarning(@delegate.Exception, $"policy: {context.PolicyKey} - message: {@delegate.Exception.Message}");
                             return Task.CompletedTask;
                         }
                    )
                    .WithPolicyKey(Constants.PolicyName.FallBack);
        }
    }
}
