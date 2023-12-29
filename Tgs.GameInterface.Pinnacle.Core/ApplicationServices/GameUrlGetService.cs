using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class GameUrlGetService(
    IGameApiClient client) : IGetService<GameUrlQuery, string>
{
    public ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
        => client.LoginV2Async(
            apiUrl: query.ApiUrl,
            agentCode: query.AgentCode,
            agentKey: query.AgentKey,
            secretKey: query.SecretKey,
            userCode: query.PlayerCode,
            locale: query.Locale,
            cancellationToken: cancellationToken);
}
