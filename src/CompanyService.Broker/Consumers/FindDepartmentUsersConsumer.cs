using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class FindDepartmentUsersConsumer : IConsumer<IFindDepartmentUsersRequest>
    {
        private readonly IDepartmentUserRepository _repository;

        private object FindUsers(IFindDepartmentUsersRequest request)
        {
            var userIds = _repository.Find(request.DepartmentId, request.SkipCount, request.TakeCount, out int totalCount).ToList();

            return IFindDepartmentUsersResponse.CreateObj(userIds, totalCount);
        }

        public FindDepartmentUsersConsumer(
           IDepartmentUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IFindDepartmentUsersRequest> context)
        {
            var departmentId = OperationResultWrapper.CreateResponse(FindUsers, context.Message);

            await context.RespondAsync<IOperationResult<IFindDepartmentUsersResponse>>(departmentId);
        }
    }
}
