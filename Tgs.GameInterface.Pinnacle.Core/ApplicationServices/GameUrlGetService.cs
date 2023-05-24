using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, string>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => _client.LoginV2Async(
            apiUrl: query.ApiUrl,
            agentCode: query.AgentCode,
            agentKey: query.AgentKey,
            secretKey: query.SecretKey,
            userCode: query.PlayerCode,
            locale: query.Locale,
            cancellationToken: cancellationToken);
}
