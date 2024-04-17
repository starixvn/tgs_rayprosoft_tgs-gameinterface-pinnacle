using NSubstitute;
using Tgs.GameInterface.Pinnacle.Core.ApplicationServices;
using Tgs.GameInterface.Pinnacle.Core.Models;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new FundInQuery(
			ApiUrl: new Uri("https://paapistg.oreo88.com"),
			AgentCode: "V0201",
			AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
			SecretKey: "zenDIdY7Knuzmeen",
			PlayerCode: "4ucmqplxb029",
			TransactionId: Guid.NewGuid().ToString("N"),
			Amount: 1m);

		var result = new WithdrawResult(
			Res: "{\"amount\":1,\"loginId\":\"4ucmqplxb029\",\"userCode\":\"V020100001\",\"availableBalance\":0.00}");
		_ = fakeGameApiClient
			 .WithdrawAsync(
				 Arg.Is(query.ApiUrl),
				 Arg.Is(query.AgentCode),
				 Arg.Is(query.AgentKey),
				 Arg.Is(query.SecretKey),
				 Arg.Is(query.PlayerCode),
				 Arg.Is(query.TransactionId),
				 Arg.Is(query.Amount),
				 Arg.Any<CancellationToken>())
			 .Returns(result);

		var client = new FundInGetService(
			fakeGameApiClient);

		var actual = await client.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal(result.Res, actual.Res);
	}
}
