using NSubstitute;
using Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameUrlQuery(
            ApiUrl: new Uri("https://paapistg.oreo88.com"),
            AgentCode: "V0201",
            AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
            SecretKey: "zenDIdY7Knuzmeen",
            PlayerCode: "4ucmqplxb029",
            Locale: "zh-cn");

        var client = new GameUrlGetService(
            fakeGameApiClient);

        _ = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        _ = await fakeGameApiClient
            .Received(1)
            .LoginV2Async(
                apiUrl: Arg.Is(query.ApiUrl),
                agentCode: Arg.Is(query.AgentCode),
                agentKey: Arg.Is(query.AgentKey),
                secretKey: Arg.Is(query.SecretKey),
                userCode: Arg.Is(query.PlayerCode),
                locale: Arg.Is(query.Locale),
                cancellationToken: Arg.Any<CancellationToken>())
            .ConfigureAwait(false);
    }
}
