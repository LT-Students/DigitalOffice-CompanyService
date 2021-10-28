using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class DisactivateUserConsumer : IConsumer<IDisactivateUserRequest>
  {
    private readonly IOfficeUserRepository _officeRepository;

    public DisactivateUserConsumer(
      IOfficeUserRepository officeRepository)
    {
      _officeRepository = officeRepository;
    }

    public Task Consume(ConsumeContext<IDisactivateUserRequest> context)
    {
      _officeRepository.RemoveAsync(context.Message.UserId, context.Message.ModifiedBy);

      return Task.FromResult(0);
    }
  }
}
