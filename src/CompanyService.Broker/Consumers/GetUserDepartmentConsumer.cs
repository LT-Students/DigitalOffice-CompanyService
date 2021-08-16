using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetUserDepartmentConsumer : IConsumer<IGetDepartmentUserRequest>
    {
        private readonly IDepartmentUserRepository _repository;

        private object GetDepartment(Guid userId)
        {
            DbDepartmentUser departmentUser = _repository.Get(userId, true);

            return IGetDepartmentUserResponse.CreateObj(
                departmentUser.DepartmentId,
                departmentUser.Department.Name,
                departmentUser.CreatedAtUtc);
        }

        public GetUserDepartmentConsumer(
            IDepartmentUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetDepartmentUserRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetDepartment, context.Message.UserId);

            await context.RespondAsync<IOperationResult<IGetDepartmentUserResponse>>(response);
        }
    }
}
