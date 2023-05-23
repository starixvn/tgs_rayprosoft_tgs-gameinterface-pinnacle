using Tgs.GameInterface.Pinnacle.Core.Models;

namespace Tgs.GameInterface.Pinnacle.Core;

public interface IGameApiClient
{
    ValueTask CreateAsync(Uri apiUrl, string agentCode, string agentKey, string secretKey, string userCode, CancellationToken cancellationToken = default);

    ValueTask<DepositResult> DepositAsync(Uri apiUrl, string agentCode, string agentKey, string secretKey, string userCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<bool> DepositWithdrawStatusAsync(Uri apiUrl, string agentCode, string agentKey, string secretKey, string transactionId, CancellationToken cancellationToken = default);

    ValueTask<InfoResult> InfoAsync(Uri apiUrl, string agentCode, string agentKey, string secretKey, string userCode, CancellationToken cancellationToken = default);

    ValueTask<string> LoginV2Async(Uri apiUrl, string agentCode, string agentKey, string secretKey, string userCode, string locale, CancellationToken cancellationToken = default);

    ValueTask LogoutAsync(Uri apiUrl, string agentCode, string agentKey, string secretKey, string userCode, CancellationToken cancellationToken = default);

    IAsyncEnumerable<WagerResult> WagersAsync(Uri apiUrl, string agentCode, string agentKey, string secretKey, DateTimeOffset dateFrom, DateTimeOffset dateTo, CancellationToken cancellationToken = default);

    ValueTask<WithdrawResult> WithdrawAsync(Uri apiUrl, string agentCode, string agentKey, string secretKey, string userCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);
}
