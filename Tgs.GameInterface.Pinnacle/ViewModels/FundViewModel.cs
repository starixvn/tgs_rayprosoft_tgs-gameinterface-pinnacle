namespace Tgs.GameInterface.Pinnacle.ViewModels;

public record FundViewModel(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string SecretKey,
    string PlayerCode,
    string TransactionId,
    decimal Amount);
