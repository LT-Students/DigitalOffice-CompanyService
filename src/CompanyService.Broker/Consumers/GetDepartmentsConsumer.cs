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
  public class GetDepartmentsConsumer : IConsumer<IGetDepartmentsRequest>
  {
    private readonly IDepartmentRepository _repository;

    private object GetDepartment(IGetDepartmentsRequest request)
    {
      List<DbDepartment> dbDepartments = new();

      List<Guid> departmentIds = request.DepartmentsIds;

      if (departmentIds != null && departmentIds.Any())
      {
        dbDepartments = _repository.Get(departmentIds, true);
      }

      return IGetDepartmentsResponse.CreateObj(
        dbDepartments.Select(
          d => new DepartmentData(
            d.Id,
            d.Name,
            d.Users.FirstOrDefault(u => u.Role == (int)DepartmentUserRole.Director)?.UserId,
            d.Users.Select(u => u.UserId).ToList()))
        .ToList());
    }

    public GetDepartmentsConsumer(
      IDepartmentRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<IGetDepartmentsRequest> context)
    {
      object departmentId = OperationResultWrapper.CreateResponse(GetDepartment, context.Message);

      await context.RespondAsync<IOperationResult<IGetDepartmentsResponse>>(departmentId);
    }
  }
}
