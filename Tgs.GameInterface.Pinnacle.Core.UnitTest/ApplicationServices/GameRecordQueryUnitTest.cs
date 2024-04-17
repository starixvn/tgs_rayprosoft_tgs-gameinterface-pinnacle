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

		_ = await client.QueryAsync(
			query)
			.ToArrayAsync()
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
			?? Enumerable.Empty<WagerResult>();
	}

	[Fact]
	public async Task Normal_Process_Open()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameRecordQuery(
			ApiUrl: new Uri("https://paapistg.oreo88.com"),
			AgentCode: "V0201",
			AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
			SecretKey: "zenDIdY7Knuzmeen",
			StartTime: DateTimeOffset.Parse("2023-05-24T17:30:00+08:00"),
			EndTime: DateTimeOffset.Parse("2023-05-24T17:45:00+08:00"));

		var wagerResults = new WagerResult[] {
			new(
				50257869,
				"Soccer",
				29,
				"4ucmqplxb029",
				WagerStatus.OPEN,
				DateTimeOffset.Parse("2023-05-23T05:33:17-04:00"),
				DateTimeOffset.UnixEpoch,
				1,
				0,
				0,
				"{\"wagerId\":50260945,\"eventId\":1573701098,\"eventName\":\"Brimbank Stallions-vs-Geelong Rangers SC\",\"parentEventName\":null,\"headToHead\":null,\"wagerDateFm\":\"2023-05-23 05:33:17\",\"eventDateFm\":\"2023-05-23 05:30:00\",\"settleDateFm\":null,\"status\":\"OPEN\",\"homeTeam\":\"Brimbank Stallions\",\"awayTeam\":\"Geelong Rangers SC\",\"selection\":\"Over\",\"handicap\":3.25,\"odds\":0.909,\"oddsFormat\":2,\"betType\":3,\"league\":\"Australia - Cup Qualifiers\",\"leagueId\":10984,\"stake\":1.0,\"sportId\":29,\"sport\":\"Soccer\",\"currencyCode\":\"CNY\",\"inplayScore\":\"0-0\",\"inPlay\":true,\"homePitcher\":null,\"awayPitcher\":null,\"homePitcherName\":null,\"awayPitcherName\":null,\"period\":0,\"cancellationStatus\":null,\"parlaySelections\":[],\"category\":null,\"toWin\":0.91,\"toRisk\":1.0,\"product\":\"SB\",\"isResettle\":null,\"parlayMixOdds\":0.909,\"wagerType\":\"single\",\"competitors\":[],\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"winLoss\":0.0,\"turnover\":0.0,\"scores\":[],\"result\":null,\"volume\":0.91,\"view\":\"D-Compact\"}" )
		};
		_ = fakeGameApiClient
			.WagersAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				agentCode: Arg.Is(query.AgentCode),
				agentKey: Arg.Is(query.AgentKey),
				secretKey: Arg.Is(query.SecretKey),
				dateFrom: Arg.Is(query.StartTime),
				dateTo: Arg.Is(query.EndTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(wagerResults.ToAsyncEnumerable());

		var client = new GameRecordQueryService(
			fakeGameApiClient);

		var actual = await client.QueryAsync(
			query)
			.ToArrayAsync()
			?? Enumerable.Empty<GameRecordResult>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
		Assert.Equal(wagerResults.Single().WagerId.ToString(), actual.Single().ExternalRoundId);
		Assert.Equal(wagerResults.Single().Sport.ToString(), actual.Single().ExternalGame);
		Assert.Equal(wagerResults.Single().SportId.ToString(), actual.Single().ExternalGameId);
		Assert.Equal(wagerResults.Single().LoginId, actual.Single().PlayerCode);
		Assert.False(actual.Single().IsClosed);
		Assert.False(actual.Single().IsVoid);
		Assert.Equal(wagerResults.Single().WagerDateFm, actual.Single().ExternalCreatedOn);
		Assert.Equal(wagerResults.Single().SettleDateFm, actual.Single().ExternalClosedOn);
		Assert.Equal(wagerResults.Single().Stake, actual.Single().Bet);
		Assert.Equal(wagerResults.Single().Turnover, actual.Single().Turnover);
		Assert.Equal(0, actual.Single().Settle);
		Assert.Equal(0, actual.Single().WinLoss);
		Assert.NotNull(actual.Single().AdditionalData);
	}

	[Fact]
	public async Task Normal_Process_Pending()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameRecordQuery(
			ApiUrl: new Uri("https://paapistg.oreo88.com"),
			AgentCode: "V0201",
			AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
			SecretKey: "zenDIdY7Knuzmeen",
			StartTime: DateTimeOffset.Parse("2023-05-24T17:30:00+08:00"),
			EndTime: DateTimeOffset.Parse("2023-05-24T17:45:00+08:00"));

		var wagerResults = new WagerResult[] {
			new(
				50257869,
				"Soccer",
				29,
				"4ucmqplxb029",
				WagerStatus.PENDING,
				DateTimeOffset.Parse("2023-05-23T05:36:17-04:00"),
				DateTimeOffset.UnixEpoch,
				1,
				0,
				0,
				"{\"wagerId\":50257869,\"eventId\":1573701098,\"eventName\":\"Brimbank Stallions-vs-Geelong Rangers SC\",\"parentEventName\":null,\"headToHead\":null,\"wagerDateFm\":\"2023-05-23 05:36:17\",\"eventDateFm\":\"2023-05-23 05:30:00\",\"settleDateFm\":null,\"status\":\"PENDING\",\"homeTeam\":\"Brimbank Stallions\",\"awayTeam\":\"Geelong Rangers SC\",\"selection\":\"Brimbank Stallions\",\"handicap\":-0.25,\"odds\":0.952,\"oddsFormat\":2,\"betType\":2,\"league\":\"Australia - Cup Qualifiers\",\"leagueId\":10984,\"stake\":1.0,\"sportId\":29,\"sport\":\"Soccer\",\"currencyCode\":\"CNY\",\"inplayScore\":\"0-0\",\"inPlay\":true,\"homePitcher\":null,\"awayPitcher\":null,\"homePitcherName\":null,\"awayPitcherName\":null,\"period\":0,\"cancellationStatus\":null,\"parlaySelections\":[],\"category\":null,\"toWin\":0.95,\"toRisk\":1.0,\"product\":\"SB\",\"isResettle\":null,\"parlayMixOdds\":0.952,\"wagerType\":\"single\",\"competitors\":[],\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"winLoss\":0.0,\"turnover\":0.0,\"scores\":[],\"result\":null,\"volume\":0.95,\"view\":\"D-Compact\"}" )
		};
		_ = fakeGameApiClient
			.WagersAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				agentCode: Arg.Is(query.AgentCode),
				agentKey: Arg.Is(query.AgentKey),
				secretKey: Arg.Is(query.SecretKey),
				dateFrom: Arg.Is(query.StartTime),
				dateTo: Arg.Is(query.EndTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(wagerResults.ToAsyncEnumerable());

		var client = new GameRecordQueryService(
			fakeGameApiClient);

		var actual = await client.QueryAsync(
			query)
			.ToArrayAsync()
			?? Enumerable.Empty<GameRecordResult>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
		Assert.Equal(wagerResults.Single().WagerId.ToString(), actual.Single().ExternalRoundId);
		Assert.Equal(wagerResults.Single().Sport.ToString(), actual.Single().ExternalGame);
		Assert.Equal(wagerResults.Single().SportId.ToString(), actual.Single().ExternalGameId);
		Assert.Equal(wagerResults.Single().LoginId, actual.Single().PlayerCode);
		Assert.False(actual.Single().IsClosed);
		Assert.False(actual.Single().IsVoid);
		Assert.Equal(wagerResults.Single().WagerDateFm, actual.Single().ExternalCreatedOn);
		Assert.Equal(wagerResults.Single().SettleDateFm, actual.Single().ExternalClosedOn);
		Assert.Equal(wagerResults.Single().Stake, actual.Single().Bet);
		Assert.Equal(wagerResults.Single().Turnover, actual.Single().Turnover);
		Assert.Equal(0, actual.Single().Settle);
		Assert.Equal(0, actual.Single().WinLoss);
		Assert.NotNull(actual.Single().AdditionalData);
	}

	[Fact]
	public async Task Normal_Process_Settled_Lose()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameRecordQuery(
			ApiUrl: new Uri("https://paapistg.oreo88.com"),
			AgentCode: "V0201",
			AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
			SecretKey: "zenDIdY7Knuzmeen",
			StartTime: DateTimeOffset.Parse("2023-05-24T19:25:00+08:00"),
			EndTime: DateTimeOffset.Parse("2023-05-24T19:30:00+08:00"));

		var wagerResults = new WagerResult[] {
			new(
				50260945,
				"Soccer",
				29,
				"4ucmqplxb029",
				WagerStatus.SETTLED,
				DateTimeOffset.Parse("2023-05-23T05:36:17-04:00"),
				DateTimeOffset.Parse("2023-05-23T07:26:20-04:00"),
				1,
				1,
				-1m,
				"{\"wagerId\":50257869,\"eventId\":1573701098,\"eventName\":\"Brimbank Stallions-vs-Geelong Rangers SC\",\"parentEventName\":null,\"headToHead\":null,\"wagerDateFm\":\"2023-05-23 05:36:17\",\"eventDateFm\":\"2023-05-23 05:30:00\",\"settleDateFm\":\"2023-05-23 07:26:20\",\"status\":\"SETTLED\",\"homeTeam\":\"Brimbank Stallions\",\"awayTeam\":\"Geelong Rangers SC\",\"selection\":\"Brimbank Stallions\",\"handicap\":-0.25,\"odds\":0.952,\"oddsFormat\":2,\"betType\":2,\"league\":\"Australia - Cup Qualifiers\",\"leagueId\":10984,\"stake\":1.0,\"sportId\":29,\"sport\":\"Soccer\",\"currencyCode\":\"CNY\",\"inplayScore\":\"0-0\",\"inPlay\":true,\"homePitcher\":null,\"awayPitcher\":null,\"homePitcherName\":null,\"awayPitcherName\":null,\"period\":0,\"cancellationStatus\":null,\"parlaySelections\":[],\"category\":null,\"toWin\":0.95,\"toRisk\":1.0,\"product\":\"SB\",\"isResettle\":false,\"parlayMixOdds\":0.952,\"wagerType\":\"single\",\"competitors\":[],\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"winLoss\":-1.0,\"turnover\":1.0,\"scores\":[{\"period\":0,\"score\":\"1-3\"},{\"period\":1,\"score\":\"0-1\"}],\"result\":\"LOSE\",\"volume\":0.95,\"view\":\"D-Compact\"}" )
		};
		_ = fakeGameApiClient
			.WagersAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				agentCode: Arg.Is(query.AgentCode),
				agentKey: Arg.Is(query.AgentKey),
				secretKey: Arg.Is(query.SecretKey),
				dateFrom: Arg.Is(query.StartTime),
				dateTo: Arg.Is(query.EndTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(wagerResults.ToAsyncEnumerable());

		var client = new GameRecordQueryService(
			fakeGameApiClient);

		var actual = await client.QueryAsync(
			query)
			.ToArrayAsync()
			?? Enumerable.Empty<GameRecordResult>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
		Assert.Equal(wagerResults.Single().WagerId.ToString(), actual.Single().ExternalRoundId);
		Assert.Equal(wagerResults.Single().Sport.ToString(), actual.Single().ExternalGame);
		Assert.Equal(wagerResults.Single().SportId.ToString(), actual.Single().ExternalGameId);
		Assert.Equal(wagerResults.Single().LoginId, actual.Single().PlayerCode);
		Assert.True(actual.Single().IsClosed);
		Assert.False(actual.Single().IsVoid);
		Assert.Equal(wagerResults.Single().WagerDateFm, actual.Single().ExternalCreatedOn);
		Assert.Equal(wagerResults.Single().SettleDateFm, actual.Single().ExternalClosedOn);
		Assert.Equal(wagerResults.Single().Stake, actual.Single().Bet);
		Assert.Equal(wagerResults.Single().Turnover, actual.Single().Turnover);
		Assert.Equal(wagerResults.Single().WinLoss + wagerResults.Single().Stake, actual.Single().Settle);
		Assert.Equal(wagerResults.Single().WinLoss, actual.Single().WinLoss);
		Assert.NotNull(actual.Single().AdditionalData);
	}

	[Fact]
	public async Task Normal_Process_Settled_Win()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameRecordQuery(
			ApiUrl: new Uri("https://paapistg.oreo88.com"),
			AgentCode: "V0201",
			AgentKey: "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
			SecretKey: "zenDIdY7Knuzmeen",
			StartTime: DateTimeOffset.Parse("2023-05-24T19:25:00+08:00"),
			EndTime: DateTimeOffset.Parse("2023-05-24T19:30:00+08:00"));

		var wagerResults = new WagerResult[] {
			new(
				50260945,
				"Soccer",
				29,
				"4ucmqplxb029",
				WagerStatus.SETTLED,
				DateTimeOffset.Parse("2023-05-23T05:33:17-04:00"),
				DateTimeOffset.Parse("2023-05-23T07:26:20-04:00"),
				1,
				1,
				0.91m,
				"{\"wagerId\":50260945,\"eventId\":1573701098,\"eventName\":\"Brimbank Stallions-vs-Geelong Rangers SC\",\"parentEventName\":null,\"headToHead\":null,\"wagerDateFm\":\"2023-05-23 05:33:17\",\"eventDateFm\":\"2023-05-23 05:30:00\",\"settleDateFm\":\"2023-05-23 07:26:20\",\"status\":\"SETTLED\",\"homeTeam\":\"Brimbank Stallions\",\"awayTeam\":\"Geelong Rangers SC\",\"selection\":\"Over\",\"handicap\":3.25,\"odds\":0.909,\"oddsFormat\":2,\"betType\":3,\"league\":\"Australia - Cup Qualifiers\",\"leagueId\":10984,\"stake\":1.0,\"sportId\":29,\"sport\":\"Soccer\",\"currencyCode\":\"CNY\",\"inplayScore\":\"0-0\",\"inPlay\":true,\"homePitcher\":null,\"awayPitcher\":null,\"homePitcherName\":null,\"awayPitcherName\":null,\"period\":0,\"cancellationStatus\":null,\"parlaySelections\":[],\"category\":null,\"toWin\":0.91,\"toRisk\":1.0,\"product\":\"SB\",\"isResettle\":false,\"parlayMixOdds\":0.909,\"wagerType\":\"single\",\"competitors\":[],\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"winLoss\":0.91,\"turnover\":1.0,\"scores\":[{\"period\":0,\"score\":\"1-3\"},{\"period\":1,\"score\":\"0-1\"}],\"result\":\"WIN\",\"volume\":0.91,\"view\":\"D-Compact\"}" )
		};
		_ = fakeGameApiClient
			.WagersAsync(
				apiUrl: Arg.Is(query.ApiUrl),
				agentCode: Arg.Is(query.AgentCode),
				agentKey: Arg.Is(query.AgentKey),
				secretKey: Arg.Is(query.SecretKey),
				dateFrom: Arg.Is(query.StartTime),
				dateTo: Arg.Is(query.EndTime),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(wagerResults.ToAsyncEnumerable());

		var client = new GameRecordQueryService(
			fakeGameApiClient);

		var actual = await client.QueryAsync(
			query)
			.ToArrayAsync()
			?? Enumerable.Empty<GameRecordResult>();

		Assert.NotNull(actual);
		Assert.True(actual.Any());
		Assert.Equal(wagerResults.Single().WagerId.ToString(), actual.Single().ExternalRoundId);
		Assert.Equal(wagerResults.Single().Sport.ToString(), actual.Single().ExternalGame);
		Assert.Equal(wagerResults.Single().SportId.ToString(), actual.Single().ExternalGameId);
		Assert.Equal(wagerResults.Single().LoginId, actual.Single().PlayerCode);
		Assert.True(actual.Single().IsClosed);
		Assert.False(actual.Single().IsVoid);
		Assert.Equal(wagerResults.Single().WagerDateFm, actual.Single().ExternalCreatedOn);
		Assert.Equal(wagerResults.Single().SettleDateFm, actual.Single().ExternalClosedOn);
		Assert.Equal(wagerResults.Single().Stake, actual.Single().Bet);
		Assert.Equal(wagerResults.Single().Turnover, actual.Single().Turnover);
		Assert.Equal(wagerResults.Single().WinLoss + wagerResults.Single().Stake, actual.Single().Settle);
		Assert.Equal(wagerResults.Single().WinLoss, actual.Single().WinLoss);
		Assert.NotNull(actual.Single().AdditionalData);
	}
}
