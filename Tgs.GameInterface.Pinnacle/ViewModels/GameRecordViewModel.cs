namespace Tgs.GameInterface.Pinnacle.ViewModels;

public record GameRecordViewModel(
	Uri ApiUrl,
	string AgentCode,
	string AgentKey,
	string SecretKey,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime);
