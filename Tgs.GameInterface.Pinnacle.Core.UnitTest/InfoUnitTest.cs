using System.Net;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest;

public class InfoUnitTest
{
    [Fact]
    public async void Authentication_Required()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var userCode = "4ucmqplxb029";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "OBAwcsdswJO0USO1s7VQczgrFpCdjf6VSI7lCVznGxgnKvMU5J7Pjc/rarpGgJuV2ro7waO1DryOKC5aW+YBUA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        var builder = new UriBuilder(new Uri(apiUrl, "/b2b/player/info"))
        {
            Port = -1
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["userCode"] = userCode;
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

        _ = await Assert.ThrowsAsync<HttpRequestException>
            (() => client.InfoAsync(
                apiUrl: apiUrl,
                agentCode: agentCode,
                agentKey: agentKey,
                secretKey: secretKey,
                userCode: userCode)
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

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "OBAwcsdswJO0USO1s7VQczgrFpCdjf6VSI7lCVznGxgnKvMU5J7Pjc/rarpGgJuV2ro7waO1DryOKC5aW+YBUA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        var builder = new UriBuilder(new Uri(apiUrl, "/b2b/player/info"))
        {
            Port = -1
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["userCode"] = userCode;
        builder.Query = query.ToString();
        string url = builder.ToString();

        var content = "{\"userCode\":\"V020100001\",\"loginId\":\"4ucmqplxb029\",\"firstName\":\"\",\"lastName\":\"\",\"status\":\"ACTIVE\",\"availableBalance\":1100.00,\"outstanding\":0.00,\"createdDate\":\"2023-05-18 02:18:40\",\"createdBy\":\"B2B\"}";

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

        var actual = await client.InfoAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            secretKey: secretKey,
            userCode: userCode);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
        Assert.Equal(expected: 1100m, actual.AvailableBalance);
        Assert.Equal(expected: 0m, actual.Outstanding);
    }

    [Fact]
    public async void The_User_Does_Not_Exist_In_The_System()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var userCode = "4ucmqplxb029";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "OBAwcsdswJO0USO1s7VQczgrFpCdjf6VSI7lCVznGxgnKvMU5J7Pjc/rarpGgJuV2ro7waO1DryOKC5aW+YBUA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        var builder = new UriBuilder(new Uri(apiUrl, "/b2b/player/info"))
        {
            Port = -1
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        query["userCode"] = userCode;
        builder.Query = query.ToString();
        string url = builder.ToString();

        var content = "{\"code\":\"104\",\"message\":\"The user does not exist in the system.\"}";

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

        var actual = await client.InfoAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            secretKey: secretKey,
            userCode: userCode);

        Assert.NotNull(actual);
        Assert.False(actual.IsPlayerExist);
        Assert.Equal(expected: 0m, actual.AvailableBalance);
        Assert.Equal(expected: 0m, actual.Outstanding);
    }
}
