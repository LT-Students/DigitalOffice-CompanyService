using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers
{
    public class DepartmentMapper : IDepartmentMapper
    {
        public Department Map(DbDepartment value)
        {
            if (value == null)
            {
                throw new ArgumentNullException();
            }

            return new Department
            {
                Id = value.Id,
                Name = value.Name,
                Description = value.Description,
                DirectorUserId = value.DirectorUserId
            };
        }
    }
}
