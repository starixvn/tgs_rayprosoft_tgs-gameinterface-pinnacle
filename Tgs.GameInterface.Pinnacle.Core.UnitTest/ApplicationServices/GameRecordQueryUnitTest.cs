using NSubstitute;
using Tgs.GameInterface.Pinnacle.Core.ApplicationServices;
using Tgs.GameInterface.Pinnacle.Core.Models;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest.ApplicationServices;

public class GameRecordQueryUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameRecordQuery(
            ApiUrl: new Uri("https://paapistg.oreo88.com"),
            AgentCode: "V0201",
            AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
            SecretKey: "zenDIdY7Knuzmeen",
            StartTime: DateTimeOffset.Parse("2023-03-13T15:45:00+08:00"),
            EndTime: DateTimeOffset.Parse("2023-03-13T16:00:00+08:00"));

        var client = new GameRecordQueryService(
            fakeGameApiClient);

        _ = await client
            .QueryAsync(
               query)
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameRecordResult>();

        _ = await fakeGameApiClient
            .Received(1)
            .WagersAsync(
                 apiUrl: Arg.Is(query.ApiUrl),
                 agentCode: Arg.Is(query.AgentCode),
                 agentKey: Arg.Is(query.AgentKey),
                 secretKey: Arg.Is(query.SecretKey),
                 dateFrom: Arg.Is(query.StartTime),
                 dateTo: Arg.Is(query.EndTime),
                 cancellationToken: Arg.Any<CancellationToken>())
            .ToArrayAsync()
            .ConfigureAwait(false)
            ?? Enumerable.Empty<WagerResult>();
    }
}
