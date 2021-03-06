﻿using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetDepartmentConsumer : IConsumer<IGetDepartmentRequest>
    {
        private readonly IDepartmentRepository _repository;

        private object GetDepartment(IGetDepartmentRequest request)
        {
            var dbDepartment = _repository.GetDepartment(request.DepartmentId, request.UserId);

            return IGetDepartmentResponse.CreateObj(dbDepartment.Id, dbDepartment.Name, dbDepartment.DirectorUserId);
        }

        public GetDepartmentConsumer(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetDepartmentRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetDepartment, context.Message);

            await context.RespondAsync<IOperationResult<IGetDepartmentResponse>>(response);
        }
    }
}
