using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.Pinnacle.Core.ApplicationServices;
using Tgs.GameInterface.Pinnacle.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Pinnacle.GrpcServices
{
    public class GameInterfacePinnacleGrpcService(
        ILogger<GameInterfacePinnacleGrpcService> logger,
        IGetService<FundInQuery, FundResult> fundInService,
        IGetService<FundOutQuery, FundResult> fundOutService,
        IGetService<FundStatusQuery, bool> fundStatusService,
        IQueryService<GameRecordQuery, GameRecordResult> gameRecordService,
        IGetService<GameUrlQuery, string> gameUrlService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        ICommandService<PlayerCreateCommand> playerCreateService,
        ICommandService<PlayerLogoutCommand> PlayerLogoutService) : GameInterfacePinnacle.GameInterfacePinnacleBase
    {
        public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
        {
            logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

            return Task.FromResult(new CheckingServiceReply
            {
                MachineName = Environment.MachineName
            });
        }

        public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
        {
            var query = new FundInQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode,
                TransactionId: request.TransactionId,
                Amount: request.Amount);

            var result = await fundInService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                Res = result.Res
            };
        }

        public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
        {
            var query = new FundOutQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode,
                TransactionId: request.TransactionId,
                Amount: request.Amount);

            var result = await fundOutService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new FundReply
            {
                Res = result.Res
            };
        }

        public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
        {
            var query = new FundStatusQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                SecretKey: request.SecretKey,
                TransactionId: request.TransactionId);

            var result = await fundStatusService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new BaseReply
            {
                State = result
            };
        }

        public override async Task<GameRecordReply> GameRecord(GameRecordRequest request, ServerCallContext context)
        {
            var query = new GameRecordQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                SecretKey: request.SecretKey,
                StartTime: request.StartTime.ToDateTimeOffset(),
                EndTime: request.EndTime.ToDateTimeOffset());

            var results = await gameRecordService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false)
                ?? Enumerable.Empty<GameRecordResult>();

            var reply = new GameRecordReply();

            if (results.Any())
            {
                foreach (var result in results)
                {
                    reply.Items.Add(new GameRecordInfo
                    {
                        ExternalRoundId = result.ExternalRoundId,
                        ExternalGame = result.ExternalGame,
                        ExternalGameId = result.ExternalGameId,
                        PlayerCode = result.PlayerCode,
                        IsClosed = result.IsClosed,
                        IsVoid = result.IsVoid,
                        ExternalCreatedOn = result.ExternalCreatedOn.ToTimestamp(),
                        ExternalClosedOn = result.ExternalClosedOn.ToTimestamp(),
                        Bet = result.Bet,
                        Turnover = result.Turnover,
                        Settle = result.Settle,
                        WinLoss = result.WinLoss,
                        AdditionalData = result.AdditionalData
                    });
                }
            }

            return reply;
        }

        public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
        {
            var query = new GameUrlQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode,
                Locale: request.Locale);

            var result = await gameUrlService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new GameUrlReply
            {
                GameUrl = result
            };
        }

        public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
        {
            var query = new PlayerBalanceQuery(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode);

            var result = await playerBalanceService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

            return new PlayerBalanceReply
            {
                IsPlayerExist = result.IsPlayerExist,
                Balance = result.Balance,
                FreeBalance = result.FreeBalance,
                OutstandingBalance = result.OutstandingBalance
            };
        }

        public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
        {
            var command = new PlayerCreateCommand(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode);

            await playerCreateService
                .ExecuteAsync(
                    command: command,
                   cancellationToken: context.CancellationToken)
               .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }

        public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
        {
            var command = new PlayerLogoutCommand(
                ApiUrl: new(request.ApiUrl),
                AgentCode: request.AgentCode,
                AgentKey: request.AgentKey,
                SecretKey: request.SecretKey,
                PlayerCode: request.PlayerCode);

            await PlayerLogoutService
                .ExecuteAsync(
                    command: command,
                   cancellationToken: context.CancellationToken)
               .ConfigureAwait(false);

            return new BaseReply
            {
                State = true
            };
        }
    }
}
