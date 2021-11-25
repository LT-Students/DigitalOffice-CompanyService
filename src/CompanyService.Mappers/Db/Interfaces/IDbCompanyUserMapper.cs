using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces
{
  [AutoInject]
  public interface IDbCompanyUserMapper
  {
    DbCompanyUser Map(ICreateCompanyUserRequest request);
  }
}
