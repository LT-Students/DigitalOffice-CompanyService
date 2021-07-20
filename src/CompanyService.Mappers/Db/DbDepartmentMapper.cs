using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using System;
using System.Linq;

namespace CompanyService.Mappers.Db
{
    public class DbDepartmentMapper : IDbDepartmentMapper
    {
        public DbDepartment Map(CreateDepartmentRequest value, Guid companyId)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var departmentId = Guid.NewGuid();

            var dbDepartment = new DbDepartment
            {
                Id = departmentId,
                CompanyId = companyId,
                Name = value.Name,
                Description = value.Description,
                IsActive = true,
                Users = value.Users?.Select(du =>
                    GetDbDepartmentUser(departmentId, du, DepartmentUserRole.Employee)).ToList()
            };

            if (value.DirectorUserId.HasValue)
            {
                dbDepartment.Users.Add(GetDbDepartmentUser(departmentId, value.DirectorUserId.Value, DepartmentUserRole.Director));
            }

            return dbDepartment;
        }

        private DbDepartmentUser GetDbDepartmentUser(Guid departmentId, Guid userId, DepartmentUserRole role)
        {
            return new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                DepartmentId = departmentId,
                UserId = userId,
                Role = (int)role,
                IsActive = true,
                StartTime = DateTime.UtcNow
            };
        }
    }
}
