using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Search;
using MassTransit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class SearchDepartmentsConsumer : IConsumer<ISearchDepartmentsRequests>
    {
        private IDepartmentRepository _repository;

        private object SearchDepartment(string text)
        {
            List<DbDepartment> departments = _repository.Search(text);

            return ISearchResponse.CreateObj(
                departments.Select(
                    d => new SearchInfo(d.Id, d.Name)).ToList());
        }

        public SearchDepartmentsConsumer(
            IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ISearchDepartmentsRequests> context)
        {
            var response = OperationResultWrapper.CreateResponse(SearchDepartment, context.Message.Value);

            await context.RespondAsync<IOperationResult<ISearchResponse>>(response);
        }
    }
}
