namespace Tgs.GameInterface.Pinnacle.Core.Models;

public record WagerResult(
    long WagerId,
    string Sport,
    int SportId,
    string UserCode,
    WagerStatus Status,
    DateTimeOffset WagerDateFm,
    DateTimeOffset SettleDateFm,
    decimal Stake,
    decimal Turnover,
    decimal WinLoss,
    string AdditionalData);
