using System.Collections.Generic;
using System.Linq;
using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Company.Filters;
using LT.DigitalOffice.Models.Broker.Models.Department;
using LT.DigitalOffice.Models.Broker.Models.Office;
using LT.DigitalOffice.Models.Broker.Models.Position;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
  public class CompanyInfoMapper : ICompanyInfoMapper
  {
    private readonly IDepartmentInfoMapper _departmentMapper;
    private readonly IPositionInfoMapper _positionMapper;
    private readonly IOfficeInfoMapper _officeMapper;

    public CompanyInfoMapper(
      IDepartmentInfoMapper departmentMapper,
      IPositionInfoMapper positionMapper,
      IOfficeInfoMapper officeMapper)
    {
      _departmentMapper = departmentMapper;
      _positionMapper = positionMapper;
      _officeMapper = officeMapper;
    }

    public CompanyInfo Map(DbCompany company,
      List<DepartmentData> departments,
      List<PositionData> positions,
      List<OfficeData> offices,
      GetCompanyFilter filter)
    {
      if (company == null)
      {
        return null;
      }

      return new CompanyInfo
      {
        Id = company.Id,
        Name = company.Name,
        Description = company.Description,
        LogoContent = company.LogoContent,
        LogoExtension = company.LogoExtension,
        Tagline = company.Tagline,
        Contacts = company.Contacts,
        Departments = departments?.Select(_departmentMapper.Map).ToList(),
        Offices = offices?.Select(_officeMapper.Map).ToList(),
        Positions = positions?.Select(_positionMapper.Map).ToList()
      };
    }
  }
}
