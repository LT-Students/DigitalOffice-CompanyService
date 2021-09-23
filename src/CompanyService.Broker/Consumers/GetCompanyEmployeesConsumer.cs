using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Constants;
using LT.DigitalOffice.Kernel.Extensions;
using LT.DigitalOffice.Models.Broker.Models.Company;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class GetCompanyEmployeesConsumer : IConsumer<IGetCompanyEmployeesRequest>
  {
    private readonly IDepartmentUserRepository _departmentUserRepository;
    private readonly IPositionUserRepository _positionUserRepository;
    private readonly IOfficeUserRepository _officeUserRepository;
    private readonly IConnectionMultiplexer _cache;

    private List<DepartmentData> GetDepartments(List<Guid> userIds)
    {
      List<DbDepartment> departments = _departmentUserRepository
        .Get(userIds)
        .Select(du => du.Department)
        .Distinct()
        .ToList();

      return departments.Select(
        d => new DepartmentData(
          d.Id,
          d.Name,
          d.Users.FirstOrDefault(u => u.Role == (int)DepartmentUserRole.Director)?.UserId,
          d.Users.Select(u => u.UserId).ToList())).ToList();
    }

    private List<PositionData> GetPositions(List<Guid> userIds)
    {
      List<DbPosition> positions = _positionUserRepository
        .Get(userIds)
        .Select(du => du.Position)
        .Distinct()
        .ToList();

      return positions.Select(
        p => new PositionData(
          p.Id,
          p.Name,
          p.Users.Select(u => u.UserId).ToList())).ToList();
    }

    private List<OfficeData> GetOffices(List<Guid> userIds)
    {
      List<DbOffice> offices = _officeUserRepository
        .Get(userIds)
        .Select(du => du.Office)
        .Distinct()
        .ToList();

      return offices.Select(
        o => new OfficeData(
          o.Id,
          o.Name,
          o.City,
          o.Name,
          o.Users.Select(u => u.UserId).ToList())).ToList();
    }

    public GetCompanyEmployeesConsumer(
      IDepartmentUserRepository departmentUserRepository,
      IPositionUserRepository positionUserRepository,
      IOfficeUserRepository officeUserRepository, 
      IConnectionMultiplexer cache)
    {
      _departmentUserRepository = departmentUserRepository;
      _positionUserRepository = positionUserRepository;
      _officeUserRepository = officeUserRepository;
      _cache = cache;
    }

    private async Task CreateCache(
      List<Guid> userIds,
      List<DepartmentData> departments,
      List<PositionData> positions,
      List<OfficeData> offices)
    {
      string key = userIds.GetRedisCacheHashCode();

      if (departments != null)
      {
        await _cache.GetDatabase(Cache.Departments).StringSetAsync(key, JsonConvert.SerializeObject(departments));
      }

      if (positions != null)
      {
        await _cache.GetDatabase(Cache.Positions).StringSetAsync(key, JsonConvert.SerializeObject(positions));
      }

      if (offices != null)
      {
        await _cache.GetDatabase(Cache.Offices).StringSetAsync(key, JsonConvert.SerializeObject(offices));
      }
    }

    public async Task Consume(ConsumeContext<IGetCompanyEmployeesRequest> context)
    {
      List<DepartmentData> departments = null;
      List<PositionData> positions = null;
      List<OfficeData> offices = null;

      if (context.Message.IncludeDepartments)
      {
        departments = GetDepartments(context.Message.UserIds);
      }

      if (context.Message.IncludePositions)
      {
        positions = GetPositions(context.Message.UserIds);
      }

      if (context.Message.IncludeOffices)
      {
        offices = GetOffices(context.Message.UserIds);
      }

      object response = OperationResultWrapper.CreateResponse((_) => IGetCompanyEmployeesResponse.CreateObj(departments, positions, offices), context);

      await context.RespondAsync<IOperationResult<IGetCompanyEmployeesResponse>>(response);

      await CreateCache(context.Message.UserIds, departments, positions, offices);
    }
  }
}
