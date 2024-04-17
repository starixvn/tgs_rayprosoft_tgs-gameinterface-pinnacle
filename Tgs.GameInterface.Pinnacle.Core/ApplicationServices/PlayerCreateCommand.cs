namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record PlayerCreateCommand(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string SecretKey,
	string PlayerCode);
