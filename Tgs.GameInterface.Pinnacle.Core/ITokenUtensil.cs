namespace Tgs.GameInterface.Pinnacle.Core;

public interface ITokenUtensil
{
	public string GenerateToken(string agentCode, string agentKey, string secretKey);
}
