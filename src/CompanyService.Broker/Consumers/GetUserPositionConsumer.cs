using System;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Responses.Company;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class GetUserPositionConsumer : IConsumer<IGetPositionRequest>
    {
        private readonly IPositionRepository _repository;

        public GetUserPositionConsumer(IPositionRepository repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<IGetPositionRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(GetUserPosition, context.Message);

            await context.RespondAsync<IOperationResult<IUserPositionResponse>>(response);
        }

        private object GetUserPosition(IGetPositionRequest request)
        {
            var dbUserPosition = _repository.GetUserPosition((Guid)request.UserId);

            return IUserPositionResponse.CreateObj(dbUserPosition.Name);
        }
    }
}