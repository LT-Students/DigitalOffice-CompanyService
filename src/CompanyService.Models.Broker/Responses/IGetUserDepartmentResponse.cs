using System;

namespace LT.DigitalOffice.Broker.Responses
{
    public interface IGetUserDepartmentResponse
    {
        Guid DepartmentId { get; }
        string Name { get; }
        DateTime StartWorkingAt { get; }

        static object CreateObj(Guid departmentId, string name, DateTime startWorkingAt)
        {
            return new
            {
                DepartmentId = departmentId,
                Name = name,
                StartWorkingAt = startWorkingAt
            };
        }
    }
}
