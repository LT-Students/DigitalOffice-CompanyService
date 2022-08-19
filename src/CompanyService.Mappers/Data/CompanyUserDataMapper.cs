using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Enums;
using LT.DigitalOffice.Models.Broker.Models.Company;

namespace LT.DigitalOffice.CompanyService.Mappers.Data
{
  public class CompanyUserDataMapper : ICompanyUserDataMapper
  {
    private readonly IContractSubjectDataMapper _contractSubjectDataMapper;

    public CompanyUserDataMapper(
      IContractSubjectDataMapper contractSubjectDataMapper)
    {
      _contractSubjectDataMapper = contractSubjectDataMapper;
    }

    public CompanyUserData Map(DbCompanyUser dbCompanyUser)
    {
      if (dbCompanyUser is null)
      {
        return null;
      }

      return new(
        dbCompanyUser.UserId,
        _contractSubjectDataMapper.Map(dbCompanyUser.ContractSubject),
        (ContractTerm)dbCompanyUser.ContractTermType,
        dbCompanyUser.Rate,
        dbCompanyUser.StartWorkingAt,
        dbCompanyUser.EndWorkingAt,
        dbCompanyUser.Probation);
    }
  }
}
