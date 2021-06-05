using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetUserDepartmentConsumer : IConsumer<IGetUserDepartmentRequest>
    {
        private readonly IDepartmentUserRepository _repository;

        private object GetDepartment(Guid userId)
        {
            DbDepartmentUser departmentUser = _repository.Get(userId, true);

            return IGetUserDepartmentResponse.CreateObj(
                departmentUser.DepartmentId,
                departmentUser.Department.Name,
                departmentUser.StartTime);
        }

        public GetUserDepartmentConsumer(
            IDepartmentUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetUserDepartmentRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetDepartment, context.Message.UserId);

            await context.RespondAsync<IOperationResult<IGetUserDepartmentResponse>>(response);
        }
    }
}
