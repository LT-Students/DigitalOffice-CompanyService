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
        private IDbOfficeUserMapper _mapper;
        private IOfficeUserRepository _officeUserRepository;

        private object ChangeUserOffice(IChangeUserOfficeRequest request)
        {
            _officeUserRepository.Remove(request.UserId, request.ChangedBy);
            _officeUserRepository.Add(_mapper.Map(request));

            return true;
        }

        public ChangeUserOfficeConsumer(
            IDbOfficeUserMapper mapper,
            IOfficeUserRepository officeUserRepository)
        {
            _mapper = mapper;
            _officeUserRepository = officeUserRepository;
        }

        public async Task Consume(ConsumeContext<IChangeUserOfficeRequest> context)
        {
            var result = OperationResultWrapper.CreateResponse(ChangeUserOffice, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(result);
        }
    }
}
