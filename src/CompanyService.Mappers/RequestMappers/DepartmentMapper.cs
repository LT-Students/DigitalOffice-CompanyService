using LT.DigitalOffice.CompanyService.Mappers.RequestMappers.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Models;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests;
using System;
using System.Linq;

namespace CompanyService.Mappers.RequestMappers
{
    public class DepartmentMapper : IDepartmentMapper
    {
        public DbDepartment Map(NewDepartmentRequest value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var departmentId = Guid.NewGuid();

            var dbDepartment = new DbDepartment
            {
                Id = departmentId,
                Name = value.Info.Name,
                Description = value.Info.Description,
                DirectorUserId = value.Info.DirectorUserId,
                IsActive = true,
                Users = value.Users?.Select(du =>
                    GetDbDepartmentUser(departmentId, du)).ToList()
            };

            return dbDepartment;
        }

        private DbDepartmentUser GetDbDepartmentUser(Guid departmentId, DepartmentUser user)
        {
            return new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                DepartmentId = departmentId,
                UserId = user.UserId,
                PositionId = user.PositionId,
                IsActive = true,
                StartTime = DateTime.UtcNow
            };
        }
    }
}
