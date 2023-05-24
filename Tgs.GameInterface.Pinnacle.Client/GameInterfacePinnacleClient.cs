using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.Pinnacle.Protos;

namespace Tgs.GameInterface.Pinnacle.Client;

internal class GameInterfacePinnacleClient : IGameInterfacePinnacleClient
{
    private readonly GameInterfacePinnacle.GameInterfacePinnacleClient _client;

    public GameInterfacePinnacleClient(GameInterfacePinnacle.GameInterfacePinnacleClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default)
        => await _client
        .FundInAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                AgentCode = agentCode,
                AgentKey = agentKey,
                SecretKey = secretKey,
                PlayerCode = playerCode,
                TransactionId = transactionId,
                Amount = amount,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default)
        => await _client
        .FundOutAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                AgentCode = agentCode,
                AgentKey = agentKey,
                SecretKey = secretKey,
                PlayerCode = playerCode,
                TransactionId = transactionId,
                Amount = amount,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string transactionId,
        CancellationToken cancellationToken = default)
    {
        var reply = await _client
        .FundStatusAsync(
            request: new FundStatusRequest
            {
                ApiUrl = apiUrl.ToString(),
                AgentCode = agentCode,
                AgentKey = agentKey,
                SecretKey = secretKey,
                TransactionId = transactionId
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        return reply.State;
    }

    public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
       [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var reply = await _client
        .GameRecordAsync(
            request: new GameRecordRequest
            {
                ApiUrl = apiUrl.ToString(),
                AgentCode = agentCode,
                AgentKey = agentKey,
                SecretKey = secretKey,
                StartTime = startTime.ToTimestamp(),
                EndTime = endTime.ToTimestamp(),
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        var gameRecords = reply.Items
            ?? Enumerable.Empty<GameRecordInfo>();

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return gameRecord;
        }
    }

    public async ValueTask<string> GameUrlAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        string locale,
        CancellationToken cancellationToken = default)
    {
        var reply = await _client
        .GameUrlAsync(
            request: new GameUrlRequest
            {
                ApiUrl = apiUrl.ToString(),
                AgentCode = agentCode,
                AgentKey = agentKey,
                SecretKey = secretKey,
                PlayerCode = playerCode,
                Locale = locale,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

        return reply.GameUrl;
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default)
        => await _client
        .PlayerBalanceAsync(
            request: new PlayerBalanceRequest
            {
                ApiUrl = apiUrl.ToString(),
                AgentCode = agentCode,
                AgentKey = agentKey,
                SecretKey = secretKey,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .PlayerCreateAsync(
            request: new PlayerCreateRequest
            {
                ApiUrl = apiUrl.ToString(),
                AgentCode = agentCode,
                AgentKey = agentKey,
                SecretKey = secretKey,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerLogoutAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string playerCode,
        CancellationToken cancellationToken = default)
        => _ = await _client
        .PlayerLogoutAsync(
            request: new PlayerLogoutRequest
            {
                ApiUrl = apiUrl.ToString(),
                AgentCode = agentCode,
                AgentKey = agentKey,
                SecretKey = secretKey,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
