using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Models.Company;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class GetCompanyEmployeesConsumer : IConsumer<IGetCompanyEmployeesRequest>
  {
    private readonly IDepartmentUserRepository _departmentUserRepository;
    private readonly IPositionUserRepository _positionUserRepository;
    private readonly IOfficeUserRepository _officeUserRepository;

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

    private object GetResponse(IGetCompanyEmployeesRequest request)
    {
      List<DepartmentData> departments = null;
      List<PositionData> positions = null;
      List<OfficeData> offices = null;

      if (request.IncludeDepartments)
      {
        departments = GetDepartments(request.UserIds);
      }

      if (request.IncludePositions)
      {
        positions = GetPositions(request.UserIds);
      }

      if (request.IncludeOffices)
      {
        offices = GetOffices(request.UserIds);
      }

      return IGetCompanyEmployeesResponse.CreateObj(departments, positions, offices);
    }

    public GetCompanyEmployeesConsumer(
      IDepartmentUserRepository departmentUserRepository,
      IPositionUserRepository positionUserRepository,
      IOfficeUserRepository officeUserRepository)
    {
      _departmentUserRepository = departmentUserRepository;
      _positionUserRepository = positionUserRepository;
      _officeUserRepository = officeUserRepository;
    }

    public async Task Consume(ConsumeContext<IGetCompanyEmployeesRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(GetResponse, context.Message);

      await context.RespondAsync<IOperationResult<IGetCompanyEmployeesResponse>>(response);
    }
  }
}
