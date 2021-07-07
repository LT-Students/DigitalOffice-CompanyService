using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Kernel.Broker;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using MassTransit;
using System.Threading.Tasks;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
    public class ChangeUserOfficeConsumer : IConsumer<IChangeUserOfficeRequest>
    {
        private IOfficeUserRepository _officeUserRepository;

        private object ChangeUserOffice(IChangeUserOfficeRequest request)
        {
            _officeUserRepository.ChangeOffice(request.UserId, request.OfficeId, request.ChangedBy);

            return true;
        }

        public ChangeUserOfficeConsumer(
            IOfficeUserRepository officeUserRepository)
        {
            _officeUserRepository = officeUserRepository;
        }

        public async Task Consume(ConsumeContext<IChangeUserOfficeRequest> context)
        {
            var result = OperationResultWrapper.CreateResponse(ChangeUserOffice, context.Message);

            await context.RespondAsync<IOperationResult<bool>>(result);
        }
    }
}
