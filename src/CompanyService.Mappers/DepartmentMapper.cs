using LT.DigitalOffice.CompanyService.Mappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using LT.DigitalOffice.Kernel.Exceptions;
using System;
using System.Collections.Generic;

namespace CompanyService.Mappers
{
    public class DepartmentMapper : IMapper<NewDepartmentRequest, DbDepartment>
    {
        public DbDepartment Map(NewDepartmentRequest value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var dbDepartment = new DbDepartment
            {
                Id = Guid.NewGuid(),
                Name = value.Info.Name,
                Description = value.Info.Description,
                DirectorUserId = value.Info.DirectorUserId,
                IsActive = true,
                Users = new List<DbDepartmentUser>()
            };

            foreach (DepartmentUser user in value.Users)
            {
                dbDepartment.Users.Add(new DbDepartmentUser
                {
                    Id = Guid.NewGuid(),
                    DepartmentId = dbDepartment.Id,
                    UserId = user.UserId,
                    PositionId = user.PositionId,
                    IsActive = true,
                    StartTime = DateTime.UtcNow
                });
            }

            return dbDepartment;
        }
    }
}
