using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class FindDepartmentsConsumer : IConsumer<IFindDepartmentsRequest>
    {
        private readonly IDepartmentRepository _repository;

        private object FindDepartment(IFindDepartmentsRequest request)
        {
            List<DbDepartment> dbDepartments = new();

            if (request.DepartmentIds != null && request.DepartmentIds.Any())
            {
                dbDepartments = _repository.Find(request.DepartmentIds);
            }

            return IFindDepartmentsResponse.CreateObj(dbDepartments.ToDictionary(x => x.Id, x => x.Name));
        }

        public FindDepartmentsConsumer(
            IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IFindDepartmentsRequest> context)
        {
            var departmentId = OperationResultWrapper.CreateResponse(FindDepartment, context.Message);

            await context.RespondAsync<IOperationResult<IFindDepartmentsResponse>>(departmentId);
        }
    }
}
