namespace Tgs.GameInterface.Pinnacle.ViewModels;

public record FundStatusViewModel(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string SecretKey,
    string TransactionId);
