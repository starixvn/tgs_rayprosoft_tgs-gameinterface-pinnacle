namespace Tgs.GameInterface.Pinnacle.Core.Models;

public record InfoResult(
    bool IsPlayerExist,
    decimal AvailableBalance,
    decimal Outstanding);
