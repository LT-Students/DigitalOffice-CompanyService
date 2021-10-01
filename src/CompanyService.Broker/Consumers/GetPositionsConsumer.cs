using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Models.Company;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class GetPositionsConsumer : IConsumer<IGetPositionsRequest>
  {
    private readonly IPositionRepository _repository;
    private readonly IConnectionMultiplexer _cache;
    private readonly IOptions<RedisConfig> _redisConfig;

    private List<PositionData> GetPosition(IGetPositionsRequest request)
    {
      List<DbPosition> positions = _repository.Get(request.PositionsIds, includeUsers: true);

      return positions.Select(
          p => new PositionData(
            p.Id,
            p.Name,
            p.Users.Select(u => u.Id).ToList()))
        .ToList();
    }

    public GetPositionsConsumer(
      IPositionRepository repository,
      IConnectionMultiplexer cache,
      IOptions<RedisConfig> redisConfig)
    {
      _repository = repository;
      _cache = cache;
      _redisConfig = redisConfig;
    }

    public async Task Consume(ConsumeContext<IGetPositionsRequest> context)
    {
      List<PositionData> positions = GetPosition(context.Message);

      object response = OperationResultWrapper.CreateResponse((_) => IGetPositionsResponse.CreateObj(positions), context.Message);

      await context.RespondAsync<IOperationResult<IGetPositionsResponse>>(response);

      await _cache.GetDatabase(Cache.Positions).StringSetAsync(
        positions.Select(p => p.Id).GetRedisCacheHashCode(),
        JsonConvert.SerializeObject(positions),
        TimeSpan.FromMinutes(_redisConfig.Value.CacheLiveInMinutes));
    }
  }
}
