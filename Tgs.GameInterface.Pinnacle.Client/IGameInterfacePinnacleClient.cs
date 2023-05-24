using Tgs.GameInterface.Pinnacle.Protos;

namespace Tgs.GameInterface.Pinnacle.Client;

public interface IGameInterfacePinnacleClient
{
    ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string transactionId,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameRecordInfo> GameRecordAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        CancellationToken cancellationToken = default);

    ValueTask<string> GameUrlAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        string locale,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default);
}
