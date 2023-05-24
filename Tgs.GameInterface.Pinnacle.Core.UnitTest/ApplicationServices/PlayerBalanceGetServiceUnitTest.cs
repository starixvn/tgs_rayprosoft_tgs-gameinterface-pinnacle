using NSubstitute;
using Tgs.GameInterface.Pinnacle.Core.ApplicationServices;
using Tgs.GameInterface.Pinnacle.Core.Models;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new PlayerBalanceQuery(
            ApiUrl: new Uri("https://paapistg.oreo88.com"),
            AgentCode: "V0201",
            AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
            SecretKey: "zenDIdY7Knuzmeen",
            PlayerCode: "4ucmqplxb029");

        var result = new InfoResult(
            IsPlayerExist: true,
            AvailableBalance: 1100m,
            Outstanding: 0m);
        _ = fakeGameApiClient
             .InfoAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 agentCode: Arg.Is(query.AgentCode),
                 agentKey: Arg.Is(query.AgentKey),
                 secretKey: Arg.Is(query.SecretKey),
                 userCode: Arg.Is(query.PlayerCode),
                 cancellationToken: Arg.Any<CancellationToken>())
             .Returns(result);

        var client = new PlayerBalanceGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
    }
}
