using System.Threading.Tasks;
using LT.DigitalOffice.Broker.Requests;
using LT.DigitalOffice.Broker.Responses;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetUserPositionConsumer : IConsumer<IGetUserPositionRequest>
    {
        private readonly IPositionRepository _repository;

        public GetUserPositionConsumer(IPositionRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetUserPositionRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetUserPosition, context.Message);

            await context.RespondAsync<IOperationResult<IUserPositionResponse>>(response);
        }

        private object GetUserPosition(IGetUserPositionRequest request)
        {
            var dbUserPosition = _repository.GetUserPosition(request.UserId);

            return new
            {
                UserPositionName = dbUserPosition.Name
            };
        }
    }
}