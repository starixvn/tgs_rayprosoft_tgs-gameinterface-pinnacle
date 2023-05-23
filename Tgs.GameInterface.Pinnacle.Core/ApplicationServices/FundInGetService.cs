using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundInGetService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
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
            Res: result.Res);
    }
}
