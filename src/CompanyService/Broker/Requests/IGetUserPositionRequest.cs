using System;

namespace LT.DigitalOffice.Broker.Requests
{
    /// <summary>
    /// DTO for mass transit. Request for getting user position.
    /// </summary>
    public interface IGetUserPositionRequest
    {
        Guid UserId { get; }
    }
}