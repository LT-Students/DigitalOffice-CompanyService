using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class ChangeUserOfficeConsumer : IConsumer<IChangeUserOfficeRequest>
    {
        private readonly IOfficeUserRepository _officeUserRepository;
        private readonly IDbOfficeUserMapper _mapper;

        private object ChangeUserOffice(IChangeUserOfficeRequest request)
        {
            _officeUserRepository.Remove(request.UserId);
            _officeUserRepository.Add(_mapper.Map(request));

            return true;
        }

        public ChangeUserOfficeConsumer(
            IOfficeUserRepository officeUserRepository,
            IDbOfficeUserMapper mapper)
        {
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
