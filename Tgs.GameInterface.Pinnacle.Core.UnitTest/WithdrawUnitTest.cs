using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.Pinnacle.Core.Exceptions;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest;

public class WithdrawUnitTest
{
    [Fact]
    public async void Authentication_Required()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var userCode = "4ucmqplxb029";
        var amount = 1m;
        var transactionId = "7DE8636EE9E3AED896C35491CADFE5A3";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "OBAwcsdswJO0USO1s7VQczgrFpCdjf6VSI7lCVznGxgnKvMU5J7Pjc/rarpGgJuV2ro7waO1DryOKC5aW+YBUA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode },
            { "amount", amount.ToString() },
            { "transactionId", transactionId },
        };

        var content = "{\"code\":401,\"message\":\"Authentication Required\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/withdraw")
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

        _ = await Assert
             .ThrowsAsync<HttpRequestException>(
            () => client.WithdrawAsync(
                apiUrl: new Uri("https://paapistg.oreo88.com"),
                agentCode: agentCode,
                agentKey: agentKey,
                secretKey: secretKey,
                userCode: userCode,
                transactionId: transactionId,
                amount: amount)
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
        var amount = 1m;
        var transactionId = "7DE8636EE9E3AED896C35491CADFE5A3";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "OBAwcsdswJO0USO1s7VQczgrFpCdjf6VSI7lCVznGxgnKvMU5J7Pjc/rarpGgJuV2ro7waO1DryOKC5aW+YBUA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode },
            { "amount", amount.ToString() },
            { "transactionId", transactionId },
        };

        var content = "{\"amount\":1,\"loginId\":\"4ucmqplxb029\",\"userCode\":\"V020100001\",\"availableBalance\":0.00}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/withdraw")
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

        var actual = await client
            .WithdrawAsync(
            apiUrl: apiUrl,
            agentCode: agentCode,
            agentKey: agentKey,
            secretKey: secretKey,
            userCode: userCode,
            transactionId: transactionId,
            amount: amount);

        Assert.NotNull(actual);
        Assert.Equal(content, actual.Res);
    }

    [Fact]
    public async void The_Login_ID_Already_Exists_In_The_System()
    {
        var apiUrl = new Uri("https://paapistg.oreo88.com");
        var agentCode = "V0201";
        var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
        var secretKey = "zenDIdY7Knuzmeen";
        var userCode = "4ucmqplxb029";
        var amount = 1m;
        var transactionId = "7DE8636EE9E3AED896C35491CADFE5A3";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "OBAwcsdswJO0USO1s7VQczgrFpCdjf6VSI7lCVznGxgnKvMU5J7Pjc/rarpGgJuV2ro7waO1DryOKC5aW+YBUA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode },
            { "amount", amount.ToString() },
            { "transactionId", transactionId },
        };

        var content = "{\"code\":\"306\",\"message\":\"TransactionId cannot be duplicated.\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/withdraw")
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

        _ = await Assert.ThrowsAsync<GameInterfaceException>(
            () => client.WithdrawAsync(
                apiUrl: apiUrl,
                agentCode: agentCode,
                agentKey: agentKey,
                secretKey: secretKey,
                userCode: userCode,
                transactionId: transactionId,
                amount: amount)
            .AsTask());
    }
}
