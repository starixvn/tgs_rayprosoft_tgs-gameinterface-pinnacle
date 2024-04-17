using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class PlayerLogoutCommandService(
	IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
	public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
		=> client.LogoutAsync(
				apiUrl: command.ApiUrl,
				agentCode: command.AgentCode,
				agentKey: command.AgentKey,
				secretKey: command.SecretKey,
				userCode: command.PlayerCode,
				cancellationToken: cancellationToken);
}
