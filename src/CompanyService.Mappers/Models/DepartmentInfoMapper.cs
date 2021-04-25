using LT.DigitalOffice.CompanyService.Mappers.Models.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.Models
{
    public class DepartmentInfoMapper : IDepartmentInfoMapper
    {
        public DepartmentInfo Map(DbDepartment value, UserInfo director, List<UserInfo> users)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new DepartmentInfo
            {
                Id = value.Id,
                Name = value.Name,
                Description = value.Description,
                Director = director,
                Users = users
            };
        }
    }
}
