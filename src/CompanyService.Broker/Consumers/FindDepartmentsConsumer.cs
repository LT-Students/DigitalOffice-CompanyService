using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using MassTransit;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class FindDepartmentsConsumer : IConsumer<IFindDepartmentsRequest>
    {
        private readonly IDepartmentRepository _repository;

        private object FindDepartment(string departmentName)
        {
            var dbDepartments = _repository
                .FindDepartments()
                .FindAll(d => d.Name.ToUpper().Contains(departmentName.ToUpper()))
                .Select(d => d.Id).ToList();

            return IDepartmentsResponse.CreateObj(dbDepartments);
        }

        public FindDepartmentsConsumer(
            IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IFindDepartmentsRequest> context)
        {
            var departmentId = OperationResultWrapper.CreateResponse(FindDepartment, context.Message.DepartmentName);

            await context.RespondAsync<IOperationResult<IDepartmentsResponse>>(departmentId);
        }
    }
}
