using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class FindDepartmentUsersConsumer : IConsumer<IGetDepartmentUsersRequest>
    {
        private readonly IDepartmentRepository _repository;

        private object FindUsers(IGetDepartmentUsersRequest request)
        {
            var userIds = _repository.FindUsers(request.DepartmentId, request.SkipCount, request.TakeCount, out int totalCount).ToList();

            return IGetDepartmentUsersResponse.CreateObj(userIds, totalCount);
        }

        public FindDepartmentUsersConsumer(
           IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetDepartmentUsersRequest> context)
        {
            var departmentId = OperationResultWrapper.CreateResponse(FindUsers, context.Message);

            await context.RespondAsync<IOperationResult<IGetDepartmentUsersResponse>>(departmentId);
        }
    }
}
