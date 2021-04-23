using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using MassTransit;
using System;
using System.Collections.Generic;
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
                .FindAll(d => d.Name.ToUpper().Contains(departmentName.ToUpper()));

            var departmentNames = new Dictionary<Guid, string>();

            foreach (var department in dbDepartments)
            {
                departmentNames.Add(department.Id, department.Name);
            }

            return IGetDepartmentsResponse.CreateObj(departmentNames);
        }

        public FindDepartmentsConsumer(
            IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IFindDepartmentsRequest> context)
        {
            var departmentId = OperationResultWrapper.CreateResponse(FindDepartment, context.Message.DepartmentName);

            await context.RespondAsync<IOperationResult<IGetDepartmentsResponse>>(departmentId);
        }
    }
}
