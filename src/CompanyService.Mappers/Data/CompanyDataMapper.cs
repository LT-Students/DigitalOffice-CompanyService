using System.Linq;
using LT.DigitalOffice.CompanyService.Mappers.Data.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Models.Company;

namespace LT.DigitalOffice.CompanyService.Mappers.Data
{
  public class CompanyDataMapper : ICompanyDataMapper
  {
    private readonly ICompanyUserDataMapper _userMapper;

    public CompanyDataMapper(ICompanyUserDataMapper userMapper)
    {
      _userMapper = userMapper;
    }

    public CompanyData Map(DbCompany company)
    {
      if (company == null)
      {
        return null;
      }

      return new CompanyData(
        company.Id,
        company.CompanyName,
        company.Users.Select(u => _userMapper.Map(u)).ToList());
    }
  }
}
