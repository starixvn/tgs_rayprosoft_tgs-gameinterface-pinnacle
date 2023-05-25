using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.Pinnacle.Core.Exceptions;
using Tgs.GameInterface.Pinnacle.Core.Extensions;
using Tgs.GameInterface.Pinnacle.Core.Models;

namespace Tgs.GameInterface.Pinnacle.Core;

internal class GameApiClient : IGameApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<GameApiClient> _logger;
    private readonly ITokenUtensil _tokenUtensil;

    public GameApiClient(
        ILogger<GameApiClient> logger,
        ITokenUtensil tokenUtensil,
        HttpClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _tokenUtensil = tokenUtensil ?? throw new ArgumentNullException(nameof(tokenUtensil));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask CreateAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string userCode,
        CancellationToken cancellationToken = default)

    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "agentCode", agentCode },
            { "loginId", userCode }
        };

        var token = _tokenUtensil.GenerateToken(agentCode, agentKey, secretKey);

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "userCode", agentCode },
            { "token", token }
        };

        var url = new Uri(apiUrl, "/b2b/player/create");
        var jsonNode = await PostAsync(
            apiUrl: url,
            parameters: parameters,
            headers: headers,
            logPrefix: "create",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(jsonNode["code"]?.GetValue<string>()))
        {
            if (!jsonNode["code"]!.GetValue<string>().Equals("111", StringComparison.OrdinalIgnoreCase))
            {
                throw new GameInterfaceException(jsonNode["code"]!.GetValue<string>());
            }
        }
    }

    public async ValueTask<DepositResult> DepositAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string userCode,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default)

    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode },
            { "amount", amount.ToString() },
            { "transactionId", transactionId },
        };

        var token = _tokenUtensil.GenerateToken(agentCode, agentKey, secretKey);

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "userCode", agentCode },
            { "token", token }
        };

        var url = new Uri(apiUrl, "/b2b/player/deposit");
        var jsonNode = await PostAsync(
            apiUrl: url,
            parameters: parameters,
            headers: headers,
            logPrefix: "deposit",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !string.IsNullOrWhiteSpace(jsonNode["code"]?.GetValue<string>())
            ? throw new GameInterfaceException(jsonNode["code"]!.GetValue<string>())
            : new DepositResult(jsonNode.ToJsonString());
    }

    public async ValueTask<bool> DepositWithdrawStatusAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string transactionId,
        CancellationToken cancellationToken = default)

    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "transactionId", transactionId }
        };

        var token = _tokenUtensil.GenerateToken(agentCode, agentKey, secretKey);

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "userCode", agentCode },
            { "token", token }
        };

        var url = new Uri(apiUrl, "/b2b/player/depositwithdraw/status");
        var jsonNode = await PostAsync(
            apiUrl: url,
            parameters: parameters,
            headers: headers,
            logPrefix: "depositwithdraw-status",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(jsonNode["code"]?.GetValue<string>()))
        {
            throw new GameInterfaceException(jsonNode["code"]!.GetValue<string>());
        }

        var status = jsonNode["status"]!.GetValue<string>();

        return status.ToUpper() switch
        {
            "SUCCESS" => true,
            "NOT_EXISTS" => false,
            "FAILED" => false,
            _ => throw new GameInterfaceException(jsonNode["status"]!.GetValue<string>())
        };
    }

    public async ValueTask<InfoResult> InfoAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string userCode,
        CancellationToken cancellationToken = default)

    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode }
        };

        var token = _tokenUtensil.GenerateToken(agentCode, agentKey, secretKey);

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "userCode", agentCode },
            { "token", token }
        };

        var url = new Uri(apiUrl, "/b2b/player/info");
        var jsonNode = await GetAsync(
            apiUrl: url,
            parameters: parameters,
            headers: headers,
            logPrefix: "info",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !string.IsNullOrWhiteSpace(jsonNode["code"]?.GetValue<string>())
            ? jsonNode["code"]!.GetValue<string>().Equals("104", StringComparison.OrdinalIgnoreCase)
                ? new InfoResult(false, 0, 0)
                : throw new GameInterfaceException(jsonNode["code"]!.GetValue<string>())
            : new InfoResult(
                true,
                jsonNode["availableBalance"]!.GetValue<decimal>(),
                jsonNode["outstanding"]!.GetValue<decimal>());
    }

    public async ValueTask<string> LoginV2Async(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string userCode,
        string locale,
        CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "loginId", userCode },
            { "locale", locale }
        };

        var token = _tokenUtensil.GenerateToken(agentCode, agentKey, secretKey);

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "userCode", agentCode },
            { "token", token }
        };

        var url = new Uri(apiUrl, "/b2b/player/loginV2");
        var jsonNode = await PostAsync(
            apiUrl: url,
            parameters: parameters,
            headers: headers,
            logPrefix: "loginV2",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !string.IsNullOrWhiteSpace(jsonNode["code"]?.GetValue<string>())
            ? throw new GameInterfaceException(jsonNode["code"]!.GetValue<string>())
            : jsonNode["loginUrl"]!.GetValue<string>();
    }

    public async ValueTask LogoutAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string userCode,
        CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode }
        };

        var tolen = _tokenUtensil.GenerateToken(agentCode, agentKey, secretKey);

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "userCode", agentCode },
            { "token", tolen }
        };

        var url = new Uri(apiUrl, "/b2b/player/logout");
        var jsonNode = await PostAsync(
            apiUrl: url,
            parameters: parameters,
            headers: headers,
            logPrefix: "logout",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(jsonNode["code"]?.GetValue<string>()))
        {
            if (!jsonNode["code"]!.GetValue<string>().Equals("104", StringComparison.OrdinalIgnoreCase))
            {
                throw new GameInterfaceException(jsonNode["code"]!.GetValue<string>());
            }
        }
    }

    public async IAsyncEnumerable<WagerResult> WagersAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        DateTimeOffset dateFrom,
        DateTimeOffset dateTo,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)

    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "dateFrom", dateFrom.DatetimeConvertToString() },
            { "dateTo", dateTo.DatetimeConvertToString() },
            { "filterBy", "update_date" }
        };

        var token = _tokenUtensil.GenerateToken(agentCode, agentKey, secretKey);

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "userCode", agentCode },
            { "token", token }
        };

        var url = new Uri(apiUrl, "/b2b//report/all-wagers");
        var jsonNode = await GetAsync(
            apiUrl: url,
            parameters: parameters,
            headers: headers,
            logPrefix: "wagers",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        JsonArray wagers;
        try
        {
            wagers = jsonNode.AsArray();
        }
        catch
        {
            throw new GameInterfaceException(jsonNode["code"]!.GetValue<string>());
        }

        if (!wagers.Any())
        {
            yield break;
        }

        foreach (var wager in jsonNode.AsArray())
        {
            if (wager is null)
            {
                continue;
            }

            yield return new WagerResult(
                wager["wagerId"]!.GetValue<long>(),
                Sport: wager["sport"]?.GetValue<object>() is null
                    ? string.Empty
                    : wager["sport"]!.GetValue<string>(),
                SportId: wager["sportId"]!.GetValue<int>(),
                LoginId: wager["loginId"]!.GetValue<string>(),
                Status: Enum.Parse<WagerStatus>(wager["status"]!.GetValue<string>()),
                WagerDateFm: wager["wagerDateFm"]!.GetValue<string>().StringConvertToDatetime(),
                SettleDateFm: wager["settleDateFm"]?.GetValue<object>() is null
                    ? DateTimeOffset.UnixEpoch
                    : wager["settleDateFm"]!.GetValue<string>().StringConvertToDatetime(),
                Stake: wager["stake"]!.GetValue<decimal>(),
                Turnover: wager["turnover"]!.GetValue<decimal>(),
                WinLoss: wager["winLoss"]!.GetValue<decimal>(),
                AdditionalData: wager.ToJsonString()
                );
        }
    }

    public async ValueTask<WithdrawResult> WithdrawAsync(
        Uri apiUrl,
        string agentCode,
        string agentKey,
        string secretKey,
        string userCode,
        string transactionId,
        decimal amount,
        CancellationToken cancellationToken = default)

    {
        Dictionary<string, string> parameters = new Dictionary<string, string>
        {
            { "userCode", userCode },
            { "amount", amount.ToString() },
            { "transactionId", transactionId },
        };

        var token = _tokenUtensil.GenerateToken(agentCode, agentKey, secretKey);

        Dictionary<string, string> headers = new Dictionary<string, string>
        {
            { "userCode", agentCode },
            { "token", token }
        };

        var url = new Uri(apiUrl, "/b2b/player/withdraw");
        var jsonNode = await PostAsync(
            apiUrl: url,
            parameters: parameters,
            headers: headers,
            logPrefix: "withdraw",
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return !string.IsNullOrWhiteSpace(jsonNode["code"]?.GetValue<string>())
            ? throw new GameInterfaceException(jsonNode["code"]!.GetValue<string>())
            : new WithdrawResult(jsonNode.ToJsonString());
    }

    private async ValueTask<JsonNode> GetAsync(
        Uri apiUrl,
        IDictionary<string, string> parameters,
        IDictionary<string, string> headers,
        string logPrefix,
        CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        _client.DefaultRequestHeaders.Clear();

        foreach (var header in headers)
        {
            _client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
        var builder = new UriBuilder(apiUrl)
        {
            Port = -1
        };
        var query = HttpUtility.ParseQueryString(builder.Query);
        foreach (var parameter in parameters)
        {
            query[parameter.Key] = parameter.Value;
        }
        builder.Query = query.ToString();
        string url = builder.ToString();
        var sw = Stopwatch.StartNew();

        var httpResponseMessage = await _client.GetAsync(url, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = string.Empty,
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }

    private async ValueTask<JsonNode> PostAsync(
            Uri apiUrl,
        IDictionary<string, string> parameters,
        IDictionary<string, string> headers,
        string logPrefix,
        CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        _client.DefaultRequestHeaders.Clear();

        foreach (var header in headers)
        {
            _client.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        var sw = Stopwatch.StartNew();

        HttpContent content = new FormUrlEncodedContent(parameters);

        var httpResponseMessage = await _client.PostAsync(apiUrl, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };
        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
