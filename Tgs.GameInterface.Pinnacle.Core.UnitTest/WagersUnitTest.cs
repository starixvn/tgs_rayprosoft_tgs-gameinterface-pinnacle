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
    public async void Normal_Process()
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

        var content = "[{\"wagerId\":6862947,\"eventName\":\"Player to Win ATP French Open ? (All In)\",\"parentEventName\":null,\"headToHead\":null,\"wagerDateFm\":\"2017-04-17 07:11:17\",\"eventDateFm\":\"2017-05-22 09:30:00\",\"settleDateFm\":null,\"status\":\"OPEN\",\"homeTeam\":\"Andy Murray\",\"awayTeam\":\"Player to Win ATP French Open ? (All In)\",\"selection\":\"Andy Murray\",\"handicap\":0,\"odds\":5.36,\"oddsFormat\":1,\"betType\":99,\"wagerType\":\"single\",\"leagueId\":9281,\"league\":\"ATP French Open\",\"stake\":10,\"sportId\":33,\"sport\":\"Tennis\",\"currencyCode\":\"CNY\",\"inplayScore\":\"\",\"inPlay\":false,\"homePitcher\":null,\"awayPitcher\":null,\"homePitcherName\":null,\"awayPitcherName\":null,\"period\":0,\"cancellationStatus\":null,\"parlaySelections\":[],\"category\":\"To Win\",\"toWin\":43.6,\"toRisk\":10,\"product\":\"SB\",\"isResettle\":null,\"parlayMixOdds\":5.36,\"competitors\":[],\"userCode\":\"Q23100000D\",\"loginId\":\"Q23100D\",\"winLoss\":0,\"commission\":0,\"scores\":[],\"turnover\":0,\"result\":null,\"volume\":10,\"view\":\"M-Asian\"}]";

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
    }
}
