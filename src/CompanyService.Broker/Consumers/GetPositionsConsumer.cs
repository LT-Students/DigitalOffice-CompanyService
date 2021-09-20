using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Models.Company;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class GetPositionsConsumer : IConsumer<IGetPositionsRequest>
  {
    private readonly IPositionRepository _repository;

    private object GetPosition(IGetPositionsRequest request)
    {
      List<DbPosition> positions = _repository.Get(request.PositionsIds, includeUsers: true);

      return IGetPositionsResponse.CreateObj(
        positions.Select(
          p => new PositionData(
            p.Id,
            p.Name,
            p.Users.Select(u => u.Id).ToList()))
        .ToList());
    }

    public GetPositionsConsumer(IPositionRepository repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<IGetPositionsRequest> context)
    {
      object response = OperationResultWrapper.CreateResponse(GetPosition, context.Message);

      await context.RespondAsync<IOperationResult<IGetPositionsResponse>>(response);
    }
  }
}
