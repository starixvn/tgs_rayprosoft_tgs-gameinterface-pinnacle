using NSubstitute;

namespace Tgs.GameInterface.Pinnacle.Core.UnitTest;

public class TokenUtensilUnitTest
{
	[Fact]
	public void GenerateToken()
	{
		var fakeSystemClock = Substitute.For<ISystemClock>();
		var fakeCryptographyUtensil = Substitute.For<ICryptographyUtensil>();

		var agentCode = "V0201";
		var agentKey = "8d0e9ac0-99d8-48c7-8307-bc328c4181f6";
		var secretKey = "zenDIdY7Knuzmeen";

		var currentTime = DateTimeOffset.Parse("2023-05-18T07:48:57.576+00:00");
		_ = fakeSystemClock
			.CurrentTime
			.Returns(currentTime);

		string timeStamp = currentTime.ToUnixTimeMilliseconds().ToString();

		var hashToken = "ed7d02f131b567f6495290b1b96b7793";
		_ = fakeCryptographyUtensil
			.Md5Encrypt(
				source: Arg.Is(agentCode + timeStamp + agentKey))
			.Returns(hashToken);

		string tokenPayLoad = string.Format("{0}|{1}|{2}", agentCode, timeStamp, hashToken);
		var token = "JgsztSWfevO/V1aRMy9J9WF6een19EGp1CbSRJSSDmtLZ3lfDHnM767ylwPhVBcsIzfWxXQw4D/vo44Z1L6rrw==";
		_ = fakeCryptographyUtensil
			.AesEncrypt(
				source: Arg.Is(tokenPayLoad),
				secretKey: Arg.Is(secretKey))
			.Returns(token);

		var utensil = new TokenUtensil(
			fakeSystemClock,
			fakeCryptographyUtensil);

		_ = utensil
		   .GenerateToken(
			   agentCode: agentCode,
			   agentKey: agentKey,
			   secretKey: secretKey);
	}
}
