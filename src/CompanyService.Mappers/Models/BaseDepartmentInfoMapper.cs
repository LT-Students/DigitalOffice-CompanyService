using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using System.Linq;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class BaseDepartmentInfoMapper : IBaseDepartmentInfoMapper
    {
        public BaseDepartmentInfo Map(DbDepartment value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new BaseDepartmentInfo
            {
                Id = value.Id,
                Name = value.Name,
                Description = value.Description.Trim().Any() ? value.Description.Trim() : null,
                DirectorUserId = value.DirectorUserId
            };
        }
    }
}
