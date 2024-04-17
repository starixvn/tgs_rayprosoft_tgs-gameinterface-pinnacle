namespace Tgs.GameInterface.Pinnacle.ViewModels;

public record PlayerBalanceViewModel(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string SecretKey,
	string PlayerCode);
