using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.CompanyUser;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Publishing.Subscriber.Company;

namespace LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbCompanyUserMapper
  {
    DbCompanyUser Map(CreateCompanyUserRequest request);

    DbCompanyUser Map(ICreateCompanyUserPublish request);
  }
}
