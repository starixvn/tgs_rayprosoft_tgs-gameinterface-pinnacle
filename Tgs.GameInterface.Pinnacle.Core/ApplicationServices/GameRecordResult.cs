namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record GameRecordResult(
	string ExternalRoundId,
	string ExternalGame,
	string ExternalGameId,
	string PlayerCode,
	bool IsClosed,
	bool IsVoid,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal Turnover,
	decimal Settle,
	decimal WinLoss,
	string AdditionalData);
