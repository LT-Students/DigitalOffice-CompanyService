using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class ChangeUserPositionConsumer : IConsumer<IChangeUserPositionRequest>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IPositionUserRepository _positionUserRepository;
        private readonly IDbPositionUserMapper _mapper;

        private object ChangePosition(IChangeUserPositionRequest request)
        {
            if (!_positionRepository.Contains(request.PositionId))
            {
                throw new BadRequestException($"No position with Id {request.PositionId}.");
            }

            _positionUserRepository.Remove(request.UserId);
            _positionUserRepository.Add(_mapper.Map(request.PositionId, request.UserId));

            return true;
        }

        public ChangeUserPositionConsumer(
            IPositionRepository positionRepository,
            IPositionUserRepository positionUserRepository,
            IDbPositionUserMapper mapper)
        {
            _positionRepository = positionRepository;
            _positionUserRepository = positionUserRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IChangeUserPositionRequest> context)
        {
            var response = OperationResultWrapper.CreateResponse(ChangePosition, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(response);
        }
    }
}
