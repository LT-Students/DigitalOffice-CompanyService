using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetDepartmentConsumer : IConsumer<IGetDepartmentRequest>
    {
        private readonly IDepartmentRepository _repository;

        private object GetDepartment(IGetDepartmentRequest request)
        {
            var dbDepartment = _repository.GetDepartment(request.DepartmentId);

            return IGetDepartmentResponse.CreateObj(dbDepartment.Id, dbDepartment.Name);
        }

        public GetDepartmentConsumer([FromServices] IDepartmentRepository repository)
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
