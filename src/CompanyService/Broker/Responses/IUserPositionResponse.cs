namespace LT.DigitalOffice.Broker.Responses
{
    /// <summary>
    /// DTO for mass transit. Response contains user position information.
    /// </summary>
    public interface IUserPositionResponse
    {
        string UserPositionName { get; }
    }
}