using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto;
using LT.DigitalOffice.Kernel.Exceptions;
using System;
using System.Collections.Generic;

namespace CompanyService.Mappers
{
    public class DepartmentMapper : IMapper<DepartmentRequest, DbDepartment>
    {
        public DbDepartment Map(DepartmentRequest value)
        {
            if (value == null)
            {
                throw new BadRequestException();
            }

            var dbDepartment = new DbDepartment
            {
                Id = Guid.NewGuid(),
                Name = value.Name,
                Description = value.Description,
                IsActive = true,
                Users = new List<DbDepartmentUser>()
            };

            foreach (Guid userId in value.UsersIds)
            {
                dbDepartment.Users.Add(new DbDepartmentUser
                {
                    DepartmentId = dbDepartment.Id,
                    UserId = userId,
                    IsActive = true,
                    StartTime = DateTime.UtcNow
                });
            }

            return dbDepartment;
        }
    }
}
