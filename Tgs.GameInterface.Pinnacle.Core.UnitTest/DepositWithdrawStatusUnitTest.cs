using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest;

public class DepositWithdrawStatusUnitTest
{
    [Fact]
    public async void FAILED()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var transactionId = "603D0BDD6EDB49ABFA54AD85B2697DD2";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "OBAwcsdswJO0USO1s7VQczgrFpCdjf6VSI7lCVznGxgnKvMU5J7Pjc/rarpGgJuV2ro7waO1DryOKC5aW+YBUA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "transactionId", transactionId }
        };

        var content = "{\"status\":\"FAILED\",\"userCode\":\"V020100001\",\"transferType\":\"DEPOSIT\",\"amount\":1.00,\"transferDate\":\"2023-05-18 04:49:54\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/depositwithdraw/status")
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

        var actual = await client.DepositWithdrawStatusAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            secretKey: secretKey,
            transactionId: transactionId);

        Assert.False(actual);
    }

    [Fact]
    public async void NOT_EXISTS()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var transactionId = "603D0BDD6EDB49ABFA54AD85B2697DD2";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "OBAwcsdswJO0USO1s7VQczgrFpCdjf6VSI7lCVznGxgnKvMU5J7Pjc/rarpGgJuV2ro7waO1DryOKC5aW+YBUA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "transactionId", transactionId }
        };

        var content = "{\"status\":\"NOT_EXISTS\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/depositwithdraw/status")
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

        var actual = await client.DepositWithdrawStatusAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            secretKey: secretKey,
            transactionId: transactionId);

        Assert.False(actual);
    }

    [Fact]
    public async void SUCCESS()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var transactionId = "603D0BDD6EDB49ABFA54AD85B2697DD2";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "OBAwcsdswJO0USO1s7VQczgrFpCdjf6VSI7lCVznGxgnKvMU5J7Pjc/rarpGgJuV2ro7waO1DryOKC5aW+YBUA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "transactionId", transactionId }
        };

        var content = "{\"status\":\"SUCCESS\",\"userCode\":\"V020100001\",\"transferType\":\"DEPOSIT\",\"amount\":1.00,\"transferDate\":\"2023-05-18 04:49:54\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/depositwithdraw/status")
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

        var actual = await client.DepositWithdrawStatusAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            secretKey: secretKey,
            transactionId: transactionId);

        Assert.True(actual);
    }
}
