using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetDepartmentsNamesConsumer : IConsumer<IGetDepartmentsNamesRequest>
    {
        private readonly IDepartmentRepository _repository;

        private object FindDepartmentNames(IList<Guid> ids)
        {
            var departmentNames = new Dictionary<Guid, string>();

            var dbDepartments = _repository.FindDepartments().FindAll(d => ids.Contains(d.Id));

            foreach (var department in dbDepartments)
            {
                departmentNames.Add(department.Id, department.Name);
            }

            return IGetDepartmentsNamesResponse.CreateObj(departmentNames);
        }

        public GetDepartmentsNamesConsumer(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetDepartmentsNamesRequest> context)
        {
            var pairs = OperationResultWrapper.CreateResponse(FindDepartmentNames, context.Message.DepartmentIds);

            await context.RespondAsync<IOperationResult<IGetDepartmentsNamesResponse>>(pairs);
        }
    }
}
