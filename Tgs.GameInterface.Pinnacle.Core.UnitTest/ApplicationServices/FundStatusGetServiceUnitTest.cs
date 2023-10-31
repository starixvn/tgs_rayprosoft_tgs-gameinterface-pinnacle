using NSubstitute;
using Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundStatusQuery(
            ApiUrl: new Uri("https://paapistg.oreo88.com"),
            AgentCode: "V0201",
            AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
            SecretKey: "zenDIdY7Knuzmeen",
            TransactionId: Guid.NewGuid().ToString("N"));

        _ = fakeGameApiClient
             .DepositWithdrawStatusAsync(
                 Arg.Is(query.ApiUrl),
                 Arg.Is(query.AgentCode),
                 Arg.Is(query.AgentKey),
                 Arg.Is(query.SecretKey),
                 Arg.Is(query.TransactionId),
                 Arg.Any<CancellationToken>())
             .Returns(true);

        var client = new FundStatusGetService(
            fakeGameApiClient);

        var actual = await client.GetAsync(query);

        Assert.True(actual);
    }
}
