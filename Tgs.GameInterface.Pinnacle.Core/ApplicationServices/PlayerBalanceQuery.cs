namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string SecretKey,
	string PlayerCode);
