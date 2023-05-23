namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record FundStatusQuery(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string SecretKey,
    string TransactionId);
