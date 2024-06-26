using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class FundInGetService(
	IGameApiClient client) : IGetService<FundInQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
	{
		var result = await client
			 .WithdrawAsync(
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
