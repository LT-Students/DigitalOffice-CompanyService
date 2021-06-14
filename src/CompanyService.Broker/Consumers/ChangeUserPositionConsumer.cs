using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class ChangeUserPositionConsumer : IConsumer<IChangeUserPositionRequest>
    {
        private readonly IPositionUserRepository _repository;
        private readonly IDbPositionUserMapper _mapper;

        private object ChangePosition(IChangeUserPositionRequest request)
        {
            _repository.Remove(request.UserId);
            bool isSuccess = _repository.Add(_mapper.Map(request.PositionId, request.UserId));

            return isSuccess;
        }

        public ChangeUserPositionConsumer(
            IPositionUserRepository repository,
            IDbPositionUserMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IChangeUserPositionRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(ChangePosition, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }
    }
}
