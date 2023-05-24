namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record GameUrlQuery(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string SecretKey,
    string PlayerCode,
    string Locale);
