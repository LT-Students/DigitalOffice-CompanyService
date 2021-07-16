using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetUserOfficesConsumer : IConsumer<IGetUserOfficesRequest>
    {
        private readonly IOfficeUserRepository _repository;

        private object GetOffices(IGetUserOfficesRequest request)
        {
            var users = _repository.Get(request.UserIds);

            List<DbOffice> office = new();

            foreach (var user in users)
            {
                if (!office.Contains(user.Office))
                {
                    office.Add(user.Office);
                }
            }

            return IGetUserOfficesResponse.CreateObj(
                office.Select(o =>
                    new OfficeData(
                        o.Id,
                        o.Name,
                        o.City,
                        o.Address,
                        users.Where(u => u.OfficeId == o.Id).Select(u => u.UserId).ToList()))
                .ToList());
        }

        public GetUserOfficesConsumer(IOfficeUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetUserOfficesRequest> context)
        {
            var result = OperationResultWrapper.CreateResponse(GetOffices, context.Message);

            await context.RespondAsync<IOperationResult<IGetUserOfficesResponse>>(result);
        }
    }
}
