using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.GetUsersDataEndpoint))]
    public interface IGetUsersDataRequest
    {
        List<Guid> UserIds { get; }

        static object CreateObj(List<Guid> userIds)
        {
            return new
            {
                UserIds = userIds
            };
        }
    }
}
