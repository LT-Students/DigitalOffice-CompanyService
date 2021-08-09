using LT.DigitalOffice.CompanyService.Data.Interfaces;
using LT.DigitalOffice.Models.Broker.Common;
using MassTransit;
using System.Threading.Tasks;

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
            _departmentUserRepository.Remove(context.Message.UserId);
            _positionUserRepository.Remove(context.Message.UserId);

            return Task.FromResult(0);
        }
    }
}
