using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class PlayerLogoutCommandService : ICommandService<PlayerLogoutCommand>
{
    private readonly IGameApiClient _client;

    public PlayerLogoutCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
        => _client.LogoutAsync(
                apiUrl: command.ApiUrl,
                agentCode: command.AgentCode,
                agentKey: command.AgentKey,
                secretKey: command.SecretKey,
                userCode: command.PlayerCode,
                cancellationToken: cancellationToken);
}
