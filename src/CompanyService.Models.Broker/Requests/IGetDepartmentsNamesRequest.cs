using LT.DigitalOffice.CompanyService.Models.Dto.Configuration;
using LT.DigitalOffice.Kernel.Attributes;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Broker.Requests
{
    [AutoInjectRequest(nameof(RabbitMqConfig.GetDepartmentsNamesEndpoint))]
    public interface IGetDepartmentsNamesRequest
    {
        IList<Guid> DepartmentIds { get; }

        static object CreateObj(IList<Guid> departmentIds)
        {
            return new
            {
                DepartmentIds = departmentIds
            };
        }
    }
}