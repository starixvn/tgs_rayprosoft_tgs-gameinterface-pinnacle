namespace Tgs.GameInterface.Pinnacle.ViewModels;

public record PlayerCreateViewModel(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string SecretKey,
    string PlayerCode);
