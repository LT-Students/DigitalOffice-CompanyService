using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Configuration
{
    public class RabbitMqConfig : BaseRabbitMqConfig
    {
        public string GetUserPositionEndpoint { get; set; }
        public string GetUsersDataEndpoint { get; set; }
        public string GetDepartmentEndpoint { get; set; }
        public string FindDepartmentUsersEndpoint { get; set; }
        public string GetDepartmentUserEndpoint { get; set; }
        public string FindDepartmentsEndpoint { get; set; }
        public string ChangeUserDepartmentEndpoint { get; set; }
        public string ChangeUserPositionEndpoint { get; set; }
        public string ChangeUserOfficeEndpoint { get; set; }
        public string GetSmtpCredentialsEndpoint { get; set; }

        [AutoInjectRequest(typeof(IGetUsersDataRequest))]
        public string GetPositionEndpoint { get; set; }

        [AutoInjectRequest(typeof(ISearchDepartmentsRequest))]
        public string SearchDepartmentEndpoint { get; set; }

        [AutoInjectRequest(typeof(IAddImageRequest))]
        public string AddImageEndpoint { get; set; }

        [AutoInjectRequest(typeof(IGetImageRequest))]
        public string GetImageEndpoint { get; set; }

        [AutoInjectRequest(typeof(ICreateAdminRequest))]
        public string CreateAdminEndpoint { get; set; }

        [AutoInjectRequest(typeof(IUpdateSmtpCredentialsRequest))]
        public string UpdateSmtpCredentialsEndpoint { get; set; }
    }
}
