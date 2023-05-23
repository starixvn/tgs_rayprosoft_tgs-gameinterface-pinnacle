namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string SecretKey,
    string PlayerCode,
    string TransactionId,
    decimal Amount);
