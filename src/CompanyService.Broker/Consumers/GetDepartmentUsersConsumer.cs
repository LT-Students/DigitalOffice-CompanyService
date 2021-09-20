﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class GetDepartmentUsersConsumer : IConsumer<IGetDepartmentUsersRequest>
  {
    private readonly IDepartmentUserRepository _repository;

    private object FindUsers(IGetDepartmentUsersRequest request)
    {
      List<Guid> userIds = _repository.Get(request, out int totalCount);

      return IGetDepartmentUsersResponse.CreateObj(userIds, totalCount);
    }

    public GetDepartmentUsersConsumer(
      IDepartmentUserRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<IGetDepartmentUsersRequest> context)
    {
      object result = OperationResultWrapper.CreateResponse(FindUsers, context.Message);

      await context.RespondAsync<IOperationResult<IGetDepartmentUsersResponse>>(result);
    }
  }
}
