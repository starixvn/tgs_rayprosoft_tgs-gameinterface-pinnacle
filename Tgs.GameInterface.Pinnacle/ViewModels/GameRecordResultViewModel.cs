namespace Tgs.GameInterface.Pinnacle.ViewModels;

public record GameRecordResultViewModel(
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
