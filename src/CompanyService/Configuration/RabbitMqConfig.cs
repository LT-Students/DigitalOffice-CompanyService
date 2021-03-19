using LT.DigitalOffice.Kernel.Broker;

namespace LT.DigitalOffice.CompanyService.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqOptions
    {
        public string GetUserPositionEndpoint { get; set; }
        public string GetDepartmentEndpoint { get; set; }
    }
}
