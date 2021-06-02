using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Attributes;
using System;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.ChangeUserDepartmentEndpoint))]
    public interface IChangeUserDepartmentRequest
    {
        Guid UserId { get; }
        Guid DepartmentId { get; }

        static object CreateObj(Guid userId, Guid departmentId)
        {
            return new
            {
                UserId = userId,
                DepartmentId = departmentId
            };
        }
    }
}
