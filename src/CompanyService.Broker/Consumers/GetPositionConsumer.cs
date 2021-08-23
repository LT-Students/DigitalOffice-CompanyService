using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetPositionConsumer : IConsumer<IGetPositionRequest>
    {
        private readonly IPositionUserRepository _repository;

        private object GetUserPosition(IGetPositionRequest request)
        {
            if (!request.UserId.HasValue)
            {
                throw new BadRequestException($"Request must contain '{nameof(request.UserId)}' value");
            }

            var positionUser = _repository.Get(request.UserId.Value)
                ?? throw new NotFoundException("This user doesn't have any position.");

            return IPositionResponse.CreateObj(
                positionUser.PositionId,
                positionUser.Position.Name,
                positionUser.CreatedAtUtc);
        }

        public GetPositionConsumer(IPositionUserRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetPositionRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetUserPosition, context.Message);

            await context.RespondAsync<IOperationResult<IPositionResponse>>(response);
        }
    }
}