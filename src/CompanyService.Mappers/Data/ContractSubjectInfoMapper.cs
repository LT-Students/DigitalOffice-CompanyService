using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;

namespace LT.DigitalOffice.CompanyService.Mappers.Data
{
  public class ContractSubjectInfoMapper : IContractSubjectInfoMapper
  {
    public ContractSubjectInfo Map(DbContractSubject contractSubject)
    {
      return new ContractSubjectInfo
      {
        Id = contractSubject.Id,
        CompanyId = contractSubject.CompanyId,
        Name = contractSubject.Name,
        Description = contractSubject.Description,
        IsActive = contractSubject.IsActive
      };
    }
  }
}
