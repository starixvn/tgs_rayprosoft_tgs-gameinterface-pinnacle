namespace Tgs.GameInterface.Pinnacle.Core;

internal class TokenUtensil(
	ISystemClock systemClock,
	ICryptographyUtensil utensil) : ITokenUtensil
{
	public string GenerateToken(string agentCode, string agentKey, string secretKey)
	{
		var currentTime = systemClock.CurrentTime;
		string timeStamp = currentTime.ToUnixTimeMilliseconds().ToString();
		string hashToken = utensil.Md5Encrypt(agentCode + timeStamp + agentKey);
		string tokenPayLoad = string.Format("{0}|{1}|{2}", agentCode, timeStamp, hashToken);
		return utensil.AesEncrypt(tokenPayLoad, secretKey);
	}
}
