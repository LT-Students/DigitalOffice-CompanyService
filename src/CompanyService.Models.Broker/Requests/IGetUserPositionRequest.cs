using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.GetPositionEndpoint))]
    public interface IGetPositionRequest
    {
        Guid? UserId { get; }
        Guid? PositionId { get; set; }

        static object CreateObj(Guid? userId, Guid? positionId)
        {
            return new
            {
                UserId = userId,
                PositionId = positionId
            };
        }
    }
}