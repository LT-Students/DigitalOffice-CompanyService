using System.Linq;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
  public class CompanyInfoMapper : ICompanyInfoMapper
  {
    //private readonly IDepartmentInfoMapper _departmentMapper;
    //private readonly IPositionInfoMapper _positionMapper;
    private readonly IOfficeInfoMapper _officeMapper;

    public CompanyInfoMapper(
      //IDepartmentInfoMapper departmentMapper,
      //IPositionInfoMapper positionMapper,
      IOfficeInfoMapper officeMapper)
    {
      //_departmentMapper = departmentMapper;
      //_positionMapper = positionMapper;
      _officeMapper = officeMapper;
    }

    public CompanyInfo Map(DbCompany company, GetCompanyFilter filter)
    {
      if (company == null)
      {
        return null;
      }

      // TODO: add directors department to response

      return new CompanyInfo
      {
        Id = company.Id,
        PortalName = company.PortalName,
        CompanyName = company.CompanyName,
        Description = company.Description,
        LogoContent = company.LogoContent,
        LogoExtension = company.LogoExtension,
        Tagline = company.Tagline,
        SiteUrl = company.SiteUrl,
        IsDepartmentModuleEnabled = company.IsDepartmentModuleEnabled,
        SmtpInfo = filter.IsIncludeSmtpCredentials ? new SmtpInfo
        {
          Port = company.Port,
          Host = company.Host,
          EnableSsl = company.EnableSsl,
          Email = company.Email,
          Password = company.Password
        } : null,
        //Departments = company?.Departments.Select(d => _departmentMapper.Map(d, null)).ToList(),
        Offices = company?.Offices.Select(o => _officeMapper.Map(o)).ToList()
        //Positions = company?.Positions.Select(p => _positionMapper.Map(p)).ToList()
      };
    }
  }
}
