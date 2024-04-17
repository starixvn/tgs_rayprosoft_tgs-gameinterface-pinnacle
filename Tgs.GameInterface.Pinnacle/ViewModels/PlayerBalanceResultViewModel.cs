namespace Tgs.GameInterface.Pinnacle.ViewModels;

public record PlayerBalanceResultViewModel(
	bool IsPlayerExist,
	decimal Balance,
	decimal FreeBalance,
	decimal OutstandingBalance);
