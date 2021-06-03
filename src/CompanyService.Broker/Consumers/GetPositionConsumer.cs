using System.Threading.Tasks;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetPositionConsumer : IConsumer<IGetPositionRequest>
    {
        private readonly IPositionRepository _repository;

        private object GetUserPosition(IGetPositionRequest request)
        {
            return new
            {
                UserPositionName = _repository.GetPosition(request.PositionId, request.UserId).Name
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