namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record PlayerLogoutCommand(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string SecretKey,
	string PlayerCode);
