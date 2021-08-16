using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Kernel.Exceptions.Models;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class ChangeUserOfficeConsumer : IConsumer<IChangeUserOfficeRequest>
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IOfficeUserRepository _officeUserRepository;
        private readonly IDbOfficeUserMapper _mapper;

        private object ChangeUserOffice(IChangeUserOfficeRequest request)
        {
            if (!_officeRepository.Contains(request.OfficeId))
            {
                throw new BadRequestException($"No office with Id {request.OfficeId}.");
            }

            _officeUserRepository.Remove(request.UserId, request.ChangedBy);
            _officeUserRepository.Add(_mapper.Map(request));

            return true;
        }

        public ChangeUserOfficeConsumer(
            IOfficeRepository officeRepository,
            IOfficeUserRepository officeUserRepository,
            IDbOfficeUserMapper mapper)
        {
            _officeRepository = officeRepository;
            _officeUserRepository = officeUserRepository;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<IChangeUserOfficeRequest> context)
        {
            var result = OperationResultWrapper.CreateResponse(ChangeUserOffice, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(result);
        }
    }
}
