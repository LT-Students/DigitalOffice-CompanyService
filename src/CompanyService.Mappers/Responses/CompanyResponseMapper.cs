using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Mappers.Responses.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using LT.DigitalOffice.Models.Broker.Models.Office;

namespace LT.DigitalOffice.CompanyService.Mappers.Responses
{
  public class CompanyResponseMapper : ICompanyResponseMapper
  {
    private readonly IOfficeInfoMapper _officeMapper;

    public CompanyResponseMapper(
      IOfficeInfoMapper officeMapper)
    {
      _officeMapper = officeMapper;
    }

    public CompanyResponse Map(DbCompany company,
      List<OfficeData> offices,
      GetCompanyFilter filter)
    {
      if (company == null)
      {
        return null;
      }

      return new CompanyResponse
      {
        Id = company.Id,
        Name = company.Name,
        Description = company.Description,
        Logo = company.LogoContent is null && company.LogoExtension is null
          ? null
          : new ImageConsist() { Content = company.LogoContent, Extension = company.LogoExtension },
        Tagline = company.Tagline,
        Contacts = company.Contacts,
        Offices = offices?.Select(_officeMapper.Map).ToList()
      };
    }
  }
}
