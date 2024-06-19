using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest;

public class CreateUnitTest
{
	[Fact]
	public async Task Authentication_Required()
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
			{ "agentCode", agentCode },
			{ "loginId", userCode }
		};

		var content = "{\"code\":401,\"message\":\"Authentication Required\"}";

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/b2b/player/create")
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
			() => client.CreateAsync(
				apiUrl: new Uri("https://paapistg.oreo88.com"),
				agentCode: agentCode,
				agentKey: agentKey,
				secretKey: secretKey,
				userCode: userCode)
			.AsTask());
	}

	[Fact]
	public async Task Normal_Process()
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
			{ "agentCode", agentCode },
			{ "loginId", userCode }
		};

		var content = "{\"loginId\":\"4ucmqplxb029\",\"userCode\":\"V020100001\"}";

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/b2b/player/create")
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

		await client.CreateAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			secretKey: secretKey,
			userCode: userCode);
	}

	[Fact]
	public async Task The_Login_ID_Already_Exists_In_The_System()
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
			{ "agentCode", agentCode },
			{ "loginId", userCode }
		};

		var content = "{\"code\":\"111\",\"message\":\"The login ID already exists in the system.\"}";

		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, "/b2b/player/create")
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

		await client.CreateAsync(
			apiUrl: apiUrl,
			agentCode: agentCode,
			agentKey: agentKey,
			secretKey: secretKey,
			userCode: userCode);
	}
}
