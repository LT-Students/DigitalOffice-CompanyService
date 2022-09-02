using LT.DigitalOffice.Kernel.BrokerSupport.Attributes;
using LT.DigitalOffice.Kernel.BrokerSupport.Configurations;
using LT.DigitalOffice.Models.Broker.Common;
using LT.DigitalOffice.Models.Broker.Requests.Image;
using LT.DigitalOffice.Models.Broker.Requests.Office;
using LT.DigitalOffice.Models.Broker.Requests.User;

namespace LT.DigitalOffice.CompanyService.Models.Dto.Configuration
{
  public class RabbitMqConfig : BaseRabbitMqConfig
  {
    public string CreateCompanyUserEndpoint { get; set; }
    public string GetCompaniesEndpoint { get; set; }
    public string DisactivateCompanyUserEndpoint { get; set; }
    public string ActivateCompanyUserEndpoint { get; set; }

    // office

    [AutoInjectRequest(typeof(IGetOfficesRequest))]
    public string GetOfficesEndpoint { get; set; }

    // user

    [AutoInjectRequest(typeof(IGetUsersDataRequest))]
    public string GetUsersDataEndpoint { get; set; }

    [AutoInjectRequest(typeof(ICheckUsersExistence))]
    public string CheckUsersExistenceEndpoint { get; set; }

    // image

    [AutoInjectRequest(typeof(IGetImagesRequest))]
    public string GetImagesEndpoint { get; set; }
  }
}
