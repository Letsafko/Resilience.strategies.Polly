using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Resilience.strategies.Polly.Test.Builder;
using Xunit;

namespace Resilience.strategies.Polly.Test
{
    public class DummyClientTests
    {
        [Fact]
        public async Task Should_retry_twice_and_break()
        {
            //Arrange
            const string expectedCalled = "called!";
            const int expectedDurationOfBreak = 10;
            const int expectedRetryAttempts = 2;
            var services = ServiceCollectionBuilder
                .Instance
                .Build();

            var client = services
                .BuildServiceProvider()
                .GetRequiredService<IDummyApiClient>();

            //Act
            var response = await client.SendAsync();
            var pollyContext = response
                .RequestMessage
                .GetPolicyExecutionContext();

            //Assert
            Assert.NotNull(pollyContext);
            Assert.Equal(expectedDurationOfBreak,
                (double)pollyContext[$"{Constants.PolicyName.CircuitBreaker}.OnBreak.DurationOfBreak"]);
            Assert.Equal(expectedRetryAttempts, (int)pollyContext[$"{Constants.PolicyName.Retry}.onRetry.Attempts"]);
            Assert.Equal(expectedCalled, pollyContext[$"{Constants.PolicyName.FallBack}.FallBackAction"]);
            Assert.Equal(expectedCalled, pollyContext[$"{Constants.PolicyName.FallBack}.OnFallBack"]);
        }
    }
}