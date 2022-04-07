using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Attributes;

namespace LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces
{
  [AutoInject]
  public interface IContractSubjectInfoMapper
  {
    ContractSubjectInfo Map(DbContractSubject contractSubject);
  }
}
