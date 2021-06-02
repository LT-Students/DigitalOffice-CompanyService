using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.ChangeUserPositionEndpoint))]
    public interface IChangeUserPositionRequest
    {
        Guid UserId { get; }
        Guid PositionId { get; }

        static object CreateObj(Guid userId, Guid positionId)
        {
            return new
            {
                UserId = userId,
                PositionId = positionId
            };
        }
    }
}
