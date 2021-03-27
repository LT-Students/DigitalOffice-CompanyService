﻿using LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.Kernel.Exceptions;

namespace LT.DigitalOffice.CompanyService.Mappers.ResponsesMappers
{
    public class DepartmentResponseMapper : IDepartmentResponseMapper
    {
        public Department Map(DbDepartment value)
        {

            if (value == null)
            {
                throw new BadRequestException();
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
