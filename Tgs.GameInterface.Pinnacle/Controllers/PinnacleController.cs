using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.Pinnacle.Core.ApplicationServices;
using Tgs.GameInterface.Pinnacle.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.Controllers
{
    [Route("pinnacle")]
    [ApiController]
    public class PinnacleController : ControllerBase
    {
        /// <summary>
        /// Funds the in asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://paapistg.oreo88.com",
        ///             "agentCode": "V0201",
        ///             "agentKey": "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
        ///             "secretKey": "zenDIdY7Knuzmeen",
        ///             "playerCode": "4ucmqplxb029",
        ///             "transactionId": "D924DB2E9E409C8AAAD6341955E3FEBE",
        ///             "amount": 1
        ///         }
        /// </remarks>
        [HttpPost("fund/in", Name = "FundIn")]
        public async ValueTask<FundResultViewModel> FundInAsync(
            [FromServices] IGetService<FundInQuery, FundResult> service,
            FundViewModel source)
        {
            var query = new FundInQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                SecretKey: source.SecretKey,
                PlayerCode: source.PlayerCode,
                TransactionId: source.TransactionId,
                Amount: source.Amount);

            var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return new FundResultViewModel(Res: result.Res);
        }

        /// <summary>
        /// Funds the out asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://paapistg.oreo88.com",
        ///             "agentCode": "V0201",
        ///             "agentKey": "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
        ///             "secretKey": "zenDIdY7Knuzmeen",
        ///             "playerCode": "4ucmqplxb029",
        ///             "transactionId": "E8E7425C214D59B22952A8E18FA9B7A4",
        ///             "amount": 1
        ///         }
        /// </remarks>
        [HttpPost("fund/out", Name = "FundOut")]
        public async ValueTask<FundResultViewModel> FundOutAsync(
            [FromServices] IGetService<FundOutQuery, FundResult> service,
            FundViewModel source)
        {
            var query = new FundOutQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                SecretKey: source.SecretKey,
                PlayerCode: source.PlayerCode,
                TransactionId: source.TransactionId,
                Amount: source.Amount);

            var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

            return new FundResultViewModel(Res: result.Res);
        }

        /// <summary>
        /// Funds the status asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://paapistg.oreo88.com",
        ///             "agentCode": "V0201",
        ///             "agentKey": "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
        ///             "secretKey": "zenDIdY7Knuzmeen",
        ///             "transactionId": "E8E7425C214D59B22952A8E18FA9B7A4"
        ///         }
        /// </remarks>
        [HttpPost("fund/Status", Name = "FundStatus")]
        public ValueTask<bool> FundStatusAsync(
            [FromServices] IGetService<FundStatusQuery, bool> service,
            FundStatusViewModel source)
        {
            var query = new FundStatusQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                SecretKey: source.SecretKey,
                TransactionId: source.TransactionId);

            return service.GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted);
        }

        /// <summary>
        /// Games the record asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://paapistg.oreo88.com",
        ///             "agentCode": "V0201",
        ///             "agentKey": "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
        ///             "secretKey": "zenDIdY7Knuzmeen",
        ///             "startTime": "2023-05-23T17:30:00+08:00",
        ///             "endTime": "2023-05-23T17:45:00+08:00"
        ///         }
        /// </remarks>
        [HttpPost("game/record")]
        public async IAsyncEnumerable<GameRecordResultViewModel> GameRecordAsync(
            [FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
            GameRecordViewModel source)
        {
            var query = new GameRecordQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                SecretKey: source.SecretKey,
                StartTime: source.StartTime,
                EndTime: source.EndTime);

            var results = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameRecordResult>();

            if (!results.Any())
            {
                yield break;
            }

            foreach (var result in results)
            {
                yield return new GameRecordResultViewModel(
                    result.ExternalRoundId,
                    result.ExternalGame,
                    result.ExternalGameId,
                    result.PlayerCode,
                    result.IsClosed,
                    result.IsVoid,
                    result.ExternalCreatedOn,
                    result.ExternalClosedOn,
                    result.Bet,
                    result.Turnover,
                    result.Settle,
                    result.WinLoss,
                    result.AdditionalData);
            }
        }

        /// <summary>
        /// Games the URL asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://paapistg.oreo88.com",
        ///             "agentCode": "V0201",
        ///             "agentKey": "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
        ///             "secretKey": "zenDIdY7Knuzmeen",
        ///             "playerCode": "4ucmqplxb029",
        ///             "locale": "zh-cn"
        ///         }
        /// </remarks>
        [HttpPost("gameurl")]
        public ValueTask<string> GameUrlAsync(
            [FromServices] IGetService<GameUrlQuery, string> service,
            GameUrlQueryViewModel source)
        {
            var query = new GameUrlQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                SecretKey: source.SecretKey,
                PlayerCode: source.PlayerCode,
                Locale: source.Locale);

            return service
             .GetAsync(
                 query: query,
                 cancellationToken: HttpContext.RequestAborted);
        }

        /// <summary>
        /// Players the balance asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://paapistg.oreo88.com",
        ///             "agentCode": "V0201",
        ///             "agentKey": "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
        ///             "secretKey": "zenDIdY7Knuzmeen",
        ///             "playerCode": "4ucmqplxb029"
        ///         }
        /// </remarks>
        [HttpPost("player/balance", Name = "PlayerBalance")]
        public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
            [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
            PlayerBalanceViewModel source)
        {
            var query = new PlayerBalanceQuery(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                SecretKey: source.SecretKey,
                PlayerCode: source.PlayerCode);

            var result = await service
                    .GetAsync(
                        query: query,
                        cancellationToken: HttpContext.RequestAborted)
                    .ConfigureAwait(false);

            return new PlayerBalanceResultViewModel(
                result.IsPlayerExist,
                result.Balance,
                result.FreeBalance,
                result.OutstandingBalance);
        }

        /// <summary>
        /// Players the create asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://paapistg.oreo88.com",
        ///             "agentCode": "V0201",
        ///             "agentKey": "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
        ///             "secretKey": "zenDIdY7Knuzmeen",
        ///             "playerCode": "4ucmqplxb029"
        ///         }
        /// </remarks>
        [HttpPost("player/create", Name = "PlayerCreate")]
        public ValueTask PlayerCreateAsync(
            [FromServices] ICommandService<PlayerCreateCommand> service,
            PlayerCreateViewModel source)
        {
            var command = new PlayerCreateCommand(
                ApiUrl: source.ApiUrl,
                AgentCode: source.AgentCode,
                AgentKey: source.AgentKey,
                SecretKey: source.SecretKey,
                PlayerCode: source.PlayerCode);
            return service.ExecuteAsync(
                    command: command,
                    cancellationToken: HttpContext.RequestAborted);
        }

        /// <summary>
        /// Players the kick asynchronous.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <remarks>
        /// <b>Example</b><br />
        ///
        ///     Post
        ///
        ///         {
        ///             "apiUrl": "https://paapistg.oreo88.com",
        ///             "agentCode": "V0201",
        ///             "agentKey": "8d0e9ac0-99d8-48c7-8307-bc328c4181f6",
        ///             "secretKey": "zenDIdY7Knuzmeen",
        ///             "playerCode": "4ucmqplxb029"
        ///         }
        /// </remarks>
        [HttpPost("player/logout", Name = "PlayerLogout")]
        public ValueTask PlayerKickAsync(
            [FromServices] ICommandService<PlayerLogoutCommand> service,
            PlayerLogoutViewModel source)
            => service
                .ExecuteAsync(
                    command: new PlayerLogoutCommand(
                        ApiUrl: source.ApiUrl,
                        AgentCode: source.AgentCode,
                        AgentKey: source.AgentKey,
                        SecretKey: source.SecretKey,
                        PlayerCode: source.PlayerCode),
                    cancellationToken: HttpContext.RequestAborted);
    }
}
