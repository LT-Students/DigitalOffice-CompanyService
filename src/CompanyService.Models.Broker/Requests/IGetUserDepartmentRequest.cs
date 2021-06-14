using System;

namespace LT.DigitalOffice.Broker.Requests
{
    public interface IGetUserDepartmentRequest
    {
        Guid UserId { get; set; }

        static object CreateObj(Guid userId)
        {
            return new
            {
                UserId = userId
            };
        }
    }
}
