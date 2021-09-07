using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class CheckDepartmentsExistenceConsumer : IConsumer<ICheckDepartmentsExistence>
  {
    private readonly IDepartmentRepository _repository;

    public CheckDepartmentsExistenceConsumer(IDepartmentRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<ICheckDepartmentsExistence> context)
    {
      var response = OperationResultWrapper.CreateResponse(GetDepartmentExistenceInfo, context.Message);

      await context.RespondAsync<IOperationResult<ICheckDepartmentsExistence>>(response);
    }

    private object GetDepartmentExistenceInfo(ICheckDepartmentsExistence requestIds)
    {
      return ICheckDepartmentsExistence.CreateObj(new List<Guid>(_repository.AreDepartmentsExist(requestIds.DepartmentIds)));
    }
  }
}
