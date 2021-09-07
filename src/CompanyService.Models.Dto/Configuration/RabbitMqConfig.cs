using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Kernel.Configurations;
using LT.DigitalOffice.Models.Broker.Requests.File;
using LT.DigitalOffice.Models.Broker.Requests.Message;
using LT.DigitalOffice.Models.Broker.Requests.Project;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Configuration
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    public string GetDepartmentsEndpoint { get; set; }
    public string GetDepartmentUsersEndpoint { get; set; }
    public string GetCompanyEmployeesEndpoint { get; set; }
    public string EditCompanyEmployeeEndpoint { get; set; }
    public string GetSmtpCredentialsEndpoint { get; set; }
    public string GetPositionsEndpoint { get; set; }
    public string DisactivateUserEndpoint { get; set; }
    public string CheckDepartmentsExistenceEndpoint { get; set; }
    public string SearchDepartmentEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetUsersDataRequest))]
    public string GetUsersDataEndpoint { get; set; }

    [AutoInjectRequest(typeof(IAddImageRequest))]
    public string AddImageEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetImagesRequest))]
    public string GetImagesEndpoint { get; set; }

    [AutoInjectRequest(typeof(IGetProjectsRequest))]
    public string GetProjectsEndpoint { get; set; }

    [AutoInjectRequest(typeof(ICreateAdminRequest))]
    public string CreateAdminEndpoint { get; set; }

    [AutoInjectRequest(typeof(IUpdateSmtpCredentialsRequest))]
    public string UpdateSmtpCredentialsEndpoint { get; set; }
  }
}
