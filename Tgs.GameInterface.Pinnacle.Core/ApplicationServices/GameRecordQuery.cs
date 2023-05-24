namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record GameRecordQuery(
    Uri ApiUrl,
    string AgentCode,
    string AgentKey,
    string SecretKey,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
