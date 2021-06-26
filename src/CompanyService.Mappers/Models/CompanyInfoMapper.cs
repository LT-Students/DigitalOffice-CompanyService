using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class CompanyInfoMapper : ICompanyInfoMapper
    {
        private readonly IShortDepartmentInfoMapper _departmentMapper;
        private readonly IPositionInfoMapper _positionMapper;
        private readonly IOfficeInfoMapper _officeMapper;

        public CompanyInfoMapper(
            IShortDepartmentInfoMapper departmentMapper,
            IPositionInfoMapper positionMapper,
            IOfficeInfoMapper officeMapper)
        {
            _departmentMapper = departmentMapper;
            _positionMapper = positionMapper;
            _officeMapper = officeMapper;
        }

        public CompanyInfo Map(DbCompany company, ImageInfo image)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            return new CompanyInfo
            {
                Id = company.Id,
                Name = company.Name,
                Description = company.Description,
                Logo = image,
                Tagline = company.Tagline,
                SiteUrl = company.SiteUrl,
                Departments = company?.Departments.Select(d => _departmentMapper.Map(d)).ToList(),
                Offices = company?.Offices.Select(o => _officeMapper.Map(o)).ToList(),
                Positions = company?.Positions.Select(p => _positionMapper.Map(p)).ToList()
            };
        }
    }
}
