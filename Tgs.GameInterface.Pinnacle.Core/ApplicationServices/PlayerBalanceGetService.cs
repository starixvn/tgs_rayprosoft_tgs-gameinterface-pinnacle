using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
            .InfoAsync(
                apiUrl: query.ApiUrl,
                agentCode: query.AgentCode,
                agentKey: query.AgentKey,
                secretKey: query.SecretKey,
                userCode: query.PlayerCode,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult
        {
            IsPlayerExist = result.IsPlayerExist,
            FreeBalance = result.AvailableBalance,
            OutstandingBalance = result.Outstanding
        };
    }
}
