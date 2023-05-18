using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest;

public class LogoutUnitTest
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

        var token = "rmqktX8oOfL/HOEBxqQnyrFwtOZVi2x/o66eEfVVpwgZmKEJ+fCCkHfqGYGi0qnuiq6LmQ76JioX+vpdfbcpdA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode }
        };

        var content = "{\"code\":401,\"message\":\"Authentication Required\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/logout")
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync().ConfigureAwait(false))
                .Respond(HttpStatusCode.Unauthorized, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        _ = await Assert
             .ThrowsAsync<HttpRequestException>(() => client
                 .LogoutAsync(
                     apiUrl: new Uri("https://paapistg.oreo88.com"),
                     agentCode: agentCode,
                     agentKey: agentKey,
                     secretKey: secretKey,
                     userCode: userCode)
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
        var userCode = "4ucmqplxb020";

        var fakeTokenUtensil = Substitute.For<ITokenUtensil>();

        var token = "rmqktX8oOfL/HOEBxqQnyrFwtOZVi2x/o66eEfVVpwgZmKEJ+fCCkHfqGYGi0qnuiq6LmQ76JioX+vpdfbcpdA==";
        _ = fakeTokenUtensil
            .GenerateToken(agentCode, agentKey, secretKey)
            .Returns(token);

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode }
        };

        var content = "{\"status\":\"successful\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/logout")
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync().ConfigureAwait(false))
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        await client
           .LogoutAsync(
               apiUrl: apiUrl,
               agentCode: agentCode,
               agentKey: agentKey,
               secretKey: secretKey,
               userCode: userCode)
           .ConfigureAwait(false);
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

        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode }
        };

        var content = "{\"code\":\"104\",\"message\":\"The user does not exist in the system.\"}";

        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, "/b2b/player/logout")
                .WithHeaders("userCode", agentCode)
                .WithHeaders("token", token)
                .WithHeaders("Content-Type", "application/x-www-form-urlencoded")
                .WithContent(await new FormUrlEncodedContent(parameters).ReadAsStringAsync().ConfigureAwait(false))
                .Respond(HttpStatusCode.OK, new StringContent(content));

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeHttpClient = handler.ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeTokenUtensil,
            fakeHttpClient);

        await client
            .LogoutAsync(
              apiUrl: apiUrl,
              agentCode: agentCode,
              agentKey: agentKey,
              secretKey: secretKey,
              userCode: userCode)
            .ConfigureAwait(false);
    }
}
