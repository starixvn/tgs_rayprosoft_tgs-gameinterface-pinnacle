namespace Tgs.GameInterface.Pinnacle.ViewModels;

public record GameUrlQueryViewModel(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string SecretKey,
    string PlayerCode,
    string Locale);
