using Microsoft.Extensions.Configuration;
using Payments.Api.Gateways;
using Shouldly;
using Xunit;

namespace Payments.UnitTests;

public class SimulatedPrimaryGatewayTests
{
    private static SimulatedPrimaryGateway CreateGateway(double failureRate)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Gateway:FailureRate"] = failureRate.ToString(System.Globalization.CultureInfo.InvariantCulture)
            })
            .Build();

        return new SimulatedPrimaryGateway(configuration);
    }

    [Fact]
    [Trait("Component", "SimulatedPrimaryGateway")]
    public async Task ChargeAsync_FailureRateIsZero_AlwaysApproves()
    {
        var gateway = CreateGateway(failureRate: 0);
        var request = new ChargeRequest(Guid.NewGuid(), 149.90m);

        var result = await gateway.ChargeAsync(request, CancellationToken.None);

        result.Success.ShouldBeTrue();
        result.Gateway.ShouldBe("primary");
        result.FailureReason.ShouldBeNull();
    }

    [Fact]
    [Trait("Component", "SimulatedPrimaryGateway")]
    public async Task ChargeAsync_FailureRateIsOne_AlwaysDeclines()
    {
        var gateway = CreateGateway(failureRate: 1);
        var request = new ChargeRequest(Guid.NewGuid(), 149.90m);

        var result = await gateway.ChargeAsync(request, CancellationToken.None);

        result.Success.ShouldBeFalse();
        result.Gateway.ShouldBe("primary");
        result.FailureReason.ShouldNotBeNullOrWhiteSpace();
    }
}
