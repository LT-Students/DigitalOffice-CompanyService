using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class ShortDepartmentInfoMapper : IShortDepartmentInfoMapper
    {
        public ShortDepartmentInfo Map(DbDepartment department)
        {
            if (department == null)
            {
                throw new ArgumentNullException(nameof(department));
            }

            return new ShortDepartmentInfo
            {
                Id = department.Id,
                Description = department.Description,
                Name = department.Name,
                IsActive = department.IsActive
            };
        }
    }
}
