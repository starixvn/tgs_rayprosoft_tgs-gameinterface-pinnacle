using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class FundStatusGetService(
    IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
    public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
        => client
        .DepositWithdrawStatusAsync(
            apiUrl: query.ApiUrl,
            agentCode: query.AgentCode,
            agentKey: query.AgentKey,
            secretKey: query.SecretKey,
            transactionId: query.TransactionId,
            cancellationToken: cancellationToken);
}
