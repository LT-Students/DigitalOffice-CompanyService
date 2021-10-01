using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
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
  public class GetDepartmentsConsumer : IConsumer<IGetDepartmentsRequest>
  {
    private readonly IDepartmentRepository _repository;
    private readonly IConnectionMultiplexer _cache;
    private readonly IOptions<RedisConfig> _redisConfig;

    private List<DepartmentData> GetDepartment(IGetDepartmentsRequest request)
    {
      List<DbDepartment> dbDepartments = new();

      List<Guid> departmentIds = request.DepartmentsIds;

      if (departmentIds != null && departmentIds.Any())
      {
        dbDepartments = _repository.Get(departmentIds, true);
      }

      return dbDepartments.Select(
          d => new DepartmentData(
            d.Id,
            d.Name,
            d.Users.FirstOrDefault(u => u.Role == (int)DepartmentUserRole.Director)?.UserId,
            d.Users.Select(u => u.UserId).ToList())).ToList();
    }

    public GetDepartmentsConsumer(
      IDepartmentRepository repository,
      IConnectionMultiplexer cache,
      IOptions<RedisConfig> redisConfig)
    {
      _repository = repository;
      _cache = cache;
      _redisConfig = redisConfig;
    }

    public async Task Consume(ConsumeContext<IGetDepartmentsRequest> context)
    {
      List<DepartmentData> departments = GetDepartment(context.Message);

      object departmentId = OperationResultWrapper.CreateResponse((_) => IGetDepartmentsResponse.CreateObj(departments), context);

      await context.RespondAsync<IOperationResult<IGetDepartmentsResponse>>(departmentId);

      await _cache.GetDatabase(Cache.Departments).StringSetAsync(
        departments.Select(d => d.Id).GetRedisCacheHashCode(),
        JsonConvert.SerializeObject(departments),
        TimeSpan.FromMinutes(_redisConfig.Value.CacheLiveInMinutes));
    }
  }
}
