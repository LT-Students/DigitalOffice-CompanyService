using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetPositionConsumer : IConsumer<IGetPositionRequest>
    {
        private readonly IPositionRepository _repository;

        private object GetUserPosition(IGetPositionRequest request)
        {
            return new
            {
                UserPositionName = _repository.Get(request.PositionId, request.UserId).Name
            };
        }

        public GetPositionConsumer(IPositionRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetPositionRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetUserPosition, context.Message);

            await context.RespondAsync<IOperationResult<IUserPositionResponse>>(response);
        }
    }
}