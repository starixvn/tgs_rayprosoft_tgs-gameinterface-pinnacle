namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string SecretKey,
    string PlayerCode,
    string TransactionId,
    decimal Amount);
