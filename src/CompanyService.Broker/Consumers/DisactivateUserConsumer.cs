using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;

namespace LT.DigitalOffice.CompanyService.Broker.Consumers
{
  public class DisactivateUserConsumer : IConsumer<IDisactivateUserRequest>
  {
    private readonly IDepartmentUserRepository _departmentUserRepository;
    private readonly IPositionUserRepository _positionUserRepository;

    public DisactivateUserConsumer(
        IDepartmentUserRepository departmentUserRepository,
        IPositionUserRepository positionUserRepository)
    {
      _departmentUserRepository = departmentUserRepository;
      _positionUserRepository = positionUserRepository;
    }

    public Task Consume(ConsumeContext<IDisactivateUserRequest> context)
    {
      _departmentUserRepository.Remove(context.Message.UserId, context.Message.ModifiedBy);
      _positionUserRepository.Remove(context.Message.UserId, context.Message.ModifiedBy);

      return Task.FromResult(0);
    }
  }
}
