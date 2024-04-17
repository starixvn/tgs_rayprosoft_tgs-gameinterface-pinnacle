using NSubstitute;
using Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var command = new PlayerLogoutCommand(
			ApiUrl: new Uri("https://paapistg.oreo88.com"),
			AgentCode: "V0201",
			AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
			SecretKey: "zenDIdY7Knuzmeen",
			PlayerCode: "4ucmqplxb029");

		var client = new PlayerLogoutCommandService(
			fakeGameApiClient);

		await client.ExecuteAsync(command);

		await fakeGameApiClient
		   .Received(1)
		   .LogoutAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			agentCode: Arg.Is(command.AgentCode),
			agentKey: Arg.Is(command.AgentKey),
			secretKey: Arg.Is(command.SecretKey),
			userCode: Arg.Is(command.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
