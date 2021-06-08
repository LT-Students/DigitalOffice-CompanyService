using LT.DigitalOffice.Kernel.Configurations;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string GetPositionEndpoint { get; set; }
        public string GetUsersDataEndpoint { get; set; }
        public string GetDepartmentEndpoint { get; set; }
        public string FindDepartmentsEndpoint { get; set; }
    }
}
