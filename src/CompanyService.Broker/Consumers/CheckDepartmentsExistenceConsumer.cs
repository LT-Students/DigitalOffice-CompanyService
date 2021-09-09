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

    private object GetDepartmentExistenceInfo(ICheckDepartmentsExistence requestIds)
    {
      return ICheckDepartmentsExistence.CreateObj(new List<Guid>(_repository.DoDepartmentsExist(requestIds.DepartmentIds)));
    }

    public CheckDepartmentsExistenceConsumer(IDepartmentRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<ICheckDepartmentsExistence> context)
    {
      object response = OperationResultWrapper.CreateResponse(GetDepartmentExistenceInfo, context.Message);

      await context.RespondAsync<IOperationResult<ICheckDepartmentsExistence>>(response);
    }
  }
}
