using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;
using LT.DigitalOffice.Models.Broker.Models.Company;

namespace LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces
{
  [AutoInject]
  public interface ICompanyDataMapper
  {
    CompanyData Map(DbCompany dbCompany);
  }
}
