using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class FundOutGetService(
	IGameApiClient client) : IGetService<FundOutQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client
		 .DepositAsync(
			 apiUrl: query.ApiUrl,
			 agentCode: query.AgentCode,
			 agentKey: query.AgentKey,
			 secretKey: query.SecretKey,
			 userCode: query.PlayerCode,
			 transactionId: query.TransactionId,
			 amount: query.Amount,
			 cancellationToken: cancellationToken)
		 .ConfigureAwait(false);

		return new FundResult(
			Resp: result.Resp);
	}
}
