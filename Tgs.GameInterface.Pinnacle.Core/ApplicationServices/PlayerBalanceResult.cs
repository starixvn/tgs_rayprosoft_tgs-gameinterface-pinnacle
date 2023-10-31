namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

public record PlayerBalanceResult
{
    public bool IsPlayerExist { get; init; } = false;
    public decimal Balance => FreeBalance + OutstandingBalance;
    public decimal FreeBalance { get; init; } = 0;
    public decimal OutstandingBalance { get; init; } = 0;
}
