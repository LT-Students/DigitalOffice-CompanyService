using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Broker.Responses
{
    public interface IDepartmentsResponse
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