using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest;

public class LoginV2UnitTest
{
    [Fact]
    public async void Authentication_Required()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var userCode = "4ucmqplxb029";
        var locale = "zh-cn";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "rmqktX8oOfL/HOEBxqQnyrFwtOZVi2x/o66eEfVVpwgZmKEJ+fCCkHfqGYGi0qnuiq6LmQ76JioX+vpdfbcpdA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "loginId", userCode },
            { "locale", locale }
        };

        var content = "{\"code\":401,\"message\":\"Authentication Required\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/loginV2")
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
                .Respond(HttpStatusCode.Unauthorized, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        _ = await Assert.ThrowsAsync<HttpRequestException>(
            () => client.LoginV2Async(
                apiUrl: new Uri("https://paapistg.oreo88.com"),
                agentCode: agentCode,
                agentKey: agentKey,
                secretKey: secretKey,
                userCode: userCode,
                locale: locale)
            .AsTask());
    }

    [Fact]
    public async void Normal_Process()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var userCode = "4ucmqplxb029";
        var locale = "zh-cn";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "rmqktX8oOfL/HOEBxqQnyrFwtOZVi2x/o66eEfVVpwgZmKEJ+fCCkHfqGYGi0qnuiq6LmQ76JioX+vpdfbcpdA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "loginId", userCode },
            { "locale", locale }
        };

        var content = "{\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"token\":\"K0hWcWI2d2p5SEtpdjY1UUxIWlZlUUtLNzdaM21BKytCK2tTbnRxa2ZyV002Y2wvZzdqZnA4dDRpN01GTzVETWU2ekJranRwUTBHcUhwOUQ5ajhBSUE9PQ==\",\"loginUrl\":\"https://pmrqhaa.oreo88.com/member-service/v1/login-token?locale=zh-cn&sport=&oddsFormat=HK&target=LIVE&token=K0hWcWI2d2p5SEtpdjY1UUxIWlZlUUtLNzdaM21BKytCK2tTbnRxa2ZyV002Y2wvZzdqZnA4dDRpN01GTzVETWU2ekJranRwUTBHcUhwOUQ5ajhBSUE9PQ==\",\"updateDate\":\"2023-05-18 03:40:12\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/loginV2")
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync())
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        var actual = await client.LoginV2Async(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            secretKey: secretKey,
            userCode: userCode,
            locale: locale);

        Assert.NotNull(actual);
        Assert.Equal("https://pmrqhaa.oreo88.com/member-service/v1/login-token?locale=zh-cn&sport=&oddsFormat=HK&target=LIVE&token=K0hWcWI2d2p5SEtpdjY1UUxIWlZlUUtLNzdaM21BKytCK2tTbnRxa2ZyV002Y2wvZzdqZnA4dDRpN01GTzVETWU2ekJranRwUTBHcUhwOUQ5ajhBSUE9PQ==", actual);
    }
}
