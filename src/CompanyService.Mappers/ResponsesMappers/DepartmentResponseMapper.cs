using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Responses;
using System;
using System.Collections.Generic;

namespace LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers
{
    public class DepartmentResponseMapper : IDepartmentResponseMapper
    {
        public DepartmentResponse Map(DbDepartment value, User director, List<User> users)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new DepartmentResponse
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
