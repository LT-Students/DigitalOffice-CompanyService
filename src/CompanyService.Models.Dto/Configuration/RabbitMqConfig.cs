using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string GetUserPositionEndpoint { get; set; }

        [AutoInjectRequest(typeof(IGetUsersDataRequest))]
        public string GetPositionEndpoint { get; set; }
        public string GetUsersDataEndpoint { get; set; }
        public string GetDepartmentEndpoint { get; set; }
        public string GetDepartmentUsersEndpoint { get; set; }
        public string GetUserDepartmentEndpoint { get; set; }
        public string FindDepartmentsEndpoint { get; set; }
        public string ChangeUserDepartmentEndpoint { get; set; }
        public string ChangeUserPositionEndpoint { get; set; }
    }
}
