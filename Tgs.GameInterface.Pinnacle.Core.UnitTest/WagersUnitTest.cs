using System.Net;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.Pinnacle.Core.Extensions;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest;

public class WagersUnitTest
{
    [Fact]
    public async void Authentication_Required()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var dateFrom = DateTimeOffset.UtcNow.AddMinutes(-10);
        var dateTo = DateTimeOffset.UtcNow;

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "rmqktX8oOfL/HOEBxqQnyrFwtOZVi2x/o66eEfVVpwgZmKEJ+fCCkHfqGYGi0qnuiq6LmQ76JioX+vpdfbcpdA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        var builder = new UriBuilder(new Uri(apiUrl, "/b2b//report/all-wagers"))
        {
            Port = -1
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["dateFrom"] = dateFrom.DatetimeConvertToString();
        query["dateTo"] = dateTo.DatetimeConvertToString();
        query["filterBy"] = "update_date";
        builder.Query = query.ToString();
        string url = builder.ToString();

        var content = "{\"code\":401,\"message\":\"Authentication Required\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url)
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .Respond(HttpStatusCode.Unauthorized, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        _ = await Assert
             .ThrowsAsync<HttpRequestException>(() => client
                 .WagersAsync(
                     apiUrl: new Uri("https://paapistg.oreo88.com"),
                     agentCode: agentCode,
                     agentKey: agentKey,
                     secretKey: secretKey,
                     dateFrom: dateFrom,
                     dateTo: dateTo)
                 .ToArrayAsync()
                 .AsTask())
              .ConfigureAwait(false);
    }

    [Fact]
    public async void Normal_Process_Pending()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var dateFrom = DateTimeOffset.UtcNow.AddMinutes(-10);
        var dateTo = DateTimeOffset.UtcNow;

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "rmqktX8oOfL/HOEBxqQnyrFwtOZVi2x/o66eEfVVpwgZmKEJ+fCCkHfqGYGi0qnuiq6LmQ76JioX+vpdfbcpdA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        var builder = new UriBuilder(new Uri(apiUrl, "/b2b//report/all-wagers"))
        {
            Port = -1
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["dateFrom"] = dateFrom.DatetimeConvertToString();
        query["dateTo"] = dateTo.DatetimeConvertToString();
        query["filterBy"] = "update_date";
        builder.Query = query.ToString();
        string url = builder.ToString();

        var content = "[{\"wagerId\":50257869,\"eventId\":1573701098,\"eventName\":\"Brimbank Stallions-vs-Geelong Rangers SC\",\"parentEventName\":null,\"headToHead\":null,\"wagerDateFm\":\"2023-05-23 05:36:17\",\"eventDateFm\":\"2023-05-23 05:30:00\",\"settleDateFm\":null,\"status\":\"PENDING\",\"homeTeam\":\"Brimbank Stallions\",\"awayTeam\":\"Geelong Rangers SC\",\"selection\":\"Brimbank Stallions\",\"handicap\":-0.25,\"odds\":0.952,\"oddsFormat\":2,\"betType\":2,\"league\":\"Australia - Cup Qualifiers\",\"leagueId\":10984,\"stake\":1.0,\"sportId\":29,\"sport\":\"Soccer\",\"currencyCode\":\"CNY\",\"inplayScore\":\"0-0\",\"inPlay\":true,\"homePitcher\":null,\"awayPitcher\":null,\"homePitcherName\":null,\"awayPitcherName\":null,\"period\":0,\"cancellationStatus\":null,\"parlaySelections\":[],\"category\":null,\"toWin\":0.95,\"toRisk\":1.0,\"product\":\"SB\",\"isResettle\":null,\"parlayMixOdds\":0.952,\"wagerType\":\"single\",\"competitors\":[],\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"winLoss\":0.0,\"turnover\":0.0,\"scores\":[],\"result\":null,\"volume\":0.95,\"view\":\"D-Compact\"}]";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url)
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        var actual = await client
            .WagersAsync(
                apiUrl: new Uri("https://paapistg.oreo88.com"),
                agentCode: agentCode,
                agentKey: agentKey,
                secretKey: secretKey,
                dateFrom: dateFrom,
                dateTo: dateTo)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(50257869, actual.Single().WagerId);
        Assert.Equal("Soccer", actual.Single().Sport);
        Assert.Equal(29, actual.Single().SportId);
        Assert.Equal("4ucmqplxb029", actual.Single().LoginId);
        Assert.Equal(WagerStatus.PENDING, actual.Single().Status);
        Assert.Equal(DateTimeOffset.Parse("2023-05-23T05:36:17-04:00"), actual.Single().WagerDateFm);
        Assert.Equal(DateTimeOffset.UnixEpoch, actual.Single().SettleDateFm);
        Assert.Equal(1m, actual.Single().Stake);
        Assert.Equal(0m, actual.Single().Turnover);
        Assert.Equal(0m, actual.Single().WinLoss);
    }

    [Fact]
    public async void Normal_Process_Open()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var dateFrom = DateTimeOffset.UtcNow.AddMinutes(-10);
        var dateTo = DateTimeOffset.UtcNow;

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "rmqktX8oOfL/HOEBxqQnyrFwtOZVi2x/o66eEfVVpwgZmKEJ+fCCkHfqGYGi0qnuiq6LmQ76JioX+vpdfbcpdA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        var builder = new UriBuilder(new Uri(apiUrl, "/b2b//report/all-wagers"))
        {
            Port = -1
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["dateFrom"] = dateFrom.DatetimeConvertToString();
        query["dateTo"] = dateTo.DatetimeConvertToString();
        query["filterBy"] = "update_date";
        builder.Query = query.ToString();
        string url = builder.ToString();

        var content = "[{\"wagerId\":50260945,\"eventId\":1573701098,\"eventName\":\"Brimbank Stallions-vs-Geelong Rangers SC\",\"parentEventName\":null,\"headToHead\":null,\"wagerDateFm\":\"2023-05-23 05:33:17\",\"eventDateFm\":\"2023-05-23 05:30:00\",\"settleDateFm\":null,\"status\":\"OPEN\",\"homeTeam\":\"Brimbank Stallions\",\"awayTeam\":\"Geelong Rangers SC\",\"selection\":\"Over\",\"handicap\":3.25,\"odds\":0.909,\"oddsFormat\":2,\"betType\":3,\"league\":\"Australia - Cup Qualifiers\",\"leagueId\":10984,\"stake\":1.0,\"sportId\":29,\"sport\":\"Soccer\",\"currencyCode\":\"CNY\",\"inplayScore\":\"0-0\",\"inPlay\":true,\"homePitcher\":null,\"awayPitcher\":null,\"homePitcherName\":null,\"awayPitcherName\":null,\"period\":0,\"cancellationStatus\":null,\"parlaySelections\":[],\"category\":null,\"toWin\":0.91,\"toRisk\":1.0,\"product\":\"SB\",\"isResettle\":null,\"parlayMixOdds\":0.909,\"wagerType\":\"single\",\"competitors\":[],\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"winLoss\":0.0,\"turnover\":0.0,\"scores\":[],\"result\":null,\"volume\":0.91,\"view\":\"D-Compact\"}]";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url)
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        var actual = await client
            .WagersAsync(
                apiUrl: new Uri("https://paapistg.oreo88.com"),
                agentCode: agentCode,
                agentKey: agentKey,
                secretKey: secretKey,
                dateFrom: dateFrom,
                dateTo: dateTo)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(50260945, actual.Single().WagerId);
        Assert.Equal("Soccer", actual.Single().Sport);
        Assert.Equal(29, actual.Single().SportId);
        Assert.Equal("4ucmqplxb029", actual.Single().LoginId);
        Assert.Equal(WagerStatus.OPEN, actual.Single().Status);
        Assert.Equal(DateTimeOffset.Parse("2023-05-23T05:33:17-04:00"), actual.Single().WagerDateFm);
        Assert.Equal(DateTimeOffset.UnixEpoch, actual.Single().SettleDateFm);
        Assert.Equal(1m, actual.Single().Stake);
        Assert.Equal(0m, actual.Single().Turnover);
        Assert.Equal(0m, actual.Single().WinLoss);
    }

    [Fact]
    public async void Normal_Process_Settled_Win()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var dateFrom = DateTimeOffset.UtcNow.AddMinutes(-10);
        var dateTo = DateTimeOffset.UtcNow;

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "rmqktX8oOfL/HOEBxqQnyrFwtOZVi2x/o66eEfVVpwgZmKEJ+fCCkHfqGYGi0qnuiq6LmQ76JioX+vpdfbcpdA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        var builder = new UriBuilder(new Uri(apiUrl, "/b2b//report/all-wagers"))
        {
            Port = -1
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["dateFrom"] = dateFrom.DatetimeConvertToString();
        query["dateTo"] = dateTo.DatetimeConvertToString();
        query["filterBy"] = "update_date";
        builder.Query = query.ToString();
        string url = builder.ToString();

        var content = "[{\"wagerId\":50260945,\"eventId\":1573701098,\"eventName\":\"Brimbank Stallions-vs-Geelong Rangers SC\",\"parentEventName\":null,\"headToHead\":null,\"wagerDateFm\":\"2023-05-23 05:33:17\",\"eventDateFm\":\"2023-05-23 05:30:00\",\"settleDateFm\":\"2023-05-23 07:26:20\",\"status\":\"SETTLED\",\"homeTeam\":\"Brimbank Stallions\",\"awayTeam\":\"Geelong Rangers SC\",\"selection\":\"Over\",\"handicap\":3.25,\"odds\":0.909,\"oddsFormat\":2,\"betType\":3,\"league\":\"Australia - Cup Qualifiers\",\"leagueId\":10984,\"stake\":1.0,\"sportId\":29,\"sport\":\"Soccer\",\"currencyCode\":\"CNY\",\"inplayScore\":\"0-0\",\"inPlay\":true,\"homePitcher\":null,\"awayPitcher\":null,\"homePitcherName\":null,\"awayPitcherName\":null,\"period\":0,\"cancellationStatus\":null,\"parlaySelections\":[],\"category\":null,\"toWin\":0.91,\"toRisk\":1.0,\"product\":\"SB\",\"isResettle\":false,\"parlayMixOdds\":0.909,\"wagerType\":\"single\",\"competitors\":[],\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"winLoss\":0.91,\"turnover\":1.0,\"scores\":[{\"period\":0,\"score\":\"1-3\"},{\"period\":1,\"score\":\"0-1\"}],\"result\":\"WIN\",\"volume\":0.91,\"view\":\"D-Compact\"}]";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url)
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        var actual = await client
            .WagersAsync(
                apiUrl: new Uri("https://paapistg.oreo88.com"),
                agentCode: agentCode,
                agentKey: agentKey,
                secretKey: secretKey,
                dateFrom: dateFrom,
                dateTo: dateTo)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(50260945, actual.Single().WagerId);
        Assert.Equal("Soccer", actual.Single().Sport);
        Assert.Equal(29, actual.Single().SportId);
        Assert.Equal("4ucmqplxb029", actual.Single().LoginId);
        Assert.Equal(WagerStatus.SETTLED, actual.Single().Status);
        Assert.Equal(DateTimeOffset.Parse("2023-05-23T05:33:17-04:00"), actual.Single().WagerDateFm);
        Assert.Equal(DateTimeOffset.Parse("2023-05-23T07:26:20-04:00"), actual.Single().SettleDateFm);
        Assert.Equal(1m, actual.Single().Stake);
        Assert.Equal(1m, actual.Single().Turnover);
        Assert.Equal(0.91m, actual.Single().WinLoss);
    }

    [Fact]
    public async void Normal_Process_Settled_Lose()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var dateFrom = DateTimeOffset.UtcNow.AddMinutes(-10);
        var dateTo = DateTimeOffset.UtcNow;

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "rmqktX8oOfL/HOEBxqQnyrFwtOZVi2x/o66eEfVVpwgZmKEJ+fCCkHfqGYGi0qnuiq6LmQ76JioX+vpdfbcpdA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        var builder = new UriBuilder(new Uri(apiUrl, "/b2b//report/all-wagers"))
        {
            Port = -1
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["dateFrom"] = dateFrom.DatetimeConvertToString();
        query["dateTo"] = dateTo.DatetimeConvertToString();
        query["filterBy"] = "update_date";
        builder.Query = query.ToString();
        string url = builder.ToString();

        var content = "[{\"wagerId\":50257869,\"eventId\":1573701098,\"eventName\":\"Brimbank Stallions-vs-Geelong Rangers SC\",\"parentEventName\":null,\"headToHead\":null,\"wagerDateFm\":\"2023-05-23 05:36:17\",\"eventDateFm\":\"2023-05-23 05:30:00\",\"settleDateFm\":\"2023-05-23 07:26:20\",\"status\":\"SETTLED\",\"homeTeam\":\"Brimbank Stallions\",\"awayTeam\":\"Geelong Rangers SC\",\"selection\":\"Brimbank Stallions\",\"handicap\":-0.25,\"odds\":0.952,\"oddsFormat\":2,\"betType\":2,\"league\":\"Australia - Cup Qualifiers\",\"leagueId\":10984,\"stake\":1.0,\"sportId\":29,\"sport\":\"Soccer\",\"currencyCode\":\"CNY\",\"inplayScore\":\"0-0\",\"inPlay\":true,\"homePitcher\":null,\"awayPitcher\":null,\"homePitcherName\":null,\"awayPitcherName\":null,\"period\":0,\"cancellationStatus\":null,\"parlaySelections\":[],\"category\":null,\"toWin\":0.95,\"toRisk\":1.0,\"product\":\"SB\",\"isResettle\":false,\"parlayMixOdds\":0.952,\"wagerType\":\"single\",\"competitors\":[],\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"winLoss\":-1.0,\"turnover\":1.0,\"scores\":[{\"period\":0,\"score\":\"1-3\"},{\"period\":1,\"score\":\"0-1\"}],\"result\":\"LOSE\",\"volume\":0.95,\"view\":\"D-Compact\"}]";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Get, url)
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        var actual = await client
            .WagersAsync(
                apiUrl: new Uri("https://paapistg.oreo88.com"),
                agentCode: agentCode,
                agentKey: agentKey,
                secretKey: secretKey,
                dateFrom: dateFrom,
                dateTo: dateTo)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(50257869, actual.Single().WagerId);
        Assert.Equal("Soccer", actual.Single().Sport);
        Assert.Equal(29, actual.Single().SportId);
        Assert.Equal("4ucmqplxb029", actual.Single().LoginId);
        Assert.Equal(WagerStatus.SETTLED, actual.Single().Status);
        Assert.Equal(DateTimeOffset.Parse("2023-05-23T05:36:17-04:00"), actual.Single().WagerDateFm);
        Assert.Equal(DateTimeOffset.Parse("2023-05-23T07:26:20-04:00"), actual.Single().SettleDateFm);
        Assert.Equal(1m, actual.Single().Stake);
        Assert.Equal(1m, actual.Single().Turnover);
        Assert.Equal(-1m, actual.Single().WinLoss);
    }
}
