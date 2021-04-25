using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.Broker.Requests
{
    public interface IFindDepartmentsRequest
    {
        string DepartmentName { get; }
        IList<Guid> DepartmentIds { get; }

        static object CreateObj(string departmentName, IList<Guid> departmentIds)
        {
            return new
            {
                DepartmentName = departmentName,
                DepartmentIds = departmentIds
            };
        }
    }
}
