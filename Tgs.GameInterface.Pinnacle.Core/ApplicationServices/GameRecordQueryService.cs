using System.Runtime.CompilerServices;
using Tgs.GameInterface.Pinnacle.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Core.ApplicationServices;

internal class GameRecordQueryService(
	IGameApiClient client) : IQueryService<GameRecordQuery, GameRecordResult>
{
	public async IAsyncEnumerable<GameRecordResult> QueryAsync(
		GameRecordQuery query,
		[EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var results = await client
			.WagersAsync(
				apiUrl: query.ApiUrl,
				agentCode: query.AgentCode,
				agentKey: query.AgentKey,
				secretKey: query.SecretKey,
				dateFrom: query.StartTime,
				dateTo: query.EndTime,
				cancellationToken: cancellationToken)
			.ToArrayAsync(
				cancellationToken)
			.ConfigureAwait(false)
			?? Enumerable.Empty<WagerResult>();

		if (!results.Any())
		{
			yield break;
		}

		foreach (var result in results)
		{
			var isClosed = false;
			var isVoid = false;
			var settle = 0.0m;
			if (result.Status is not WagerStatus.OPEN and not WagerStatus.PENDING)
			{
				isClosed = true;
				isVoid = result.Status == WagerStatus.CANCELLED;
				if (isClosed && !isVoid)
				{
					settle = result.WinLoss + result.Stake;
				}
			}

			yield return new GameRecordResult(
				ExternalRoundId: result.WagerId.ToString(),
				ExternalGame: result.Sport,
				ExternalGameId: result.SportId.ToString(),
				PlayerCode: result.LoginId,
				IsClosed: isClosed,
				IsVoid: isVoid,
				ExternalCreatedOn: result.WagerDateFm,
				ExternalClosedOn: result.SettleDateFm,
				Bet: result.Stake,
				Turnover: result.Turnover,
				Settle: settle,
				WinLoss: result.WinLoss,
				AdditionalData: result.AdditionalData);
		}
	}
}
