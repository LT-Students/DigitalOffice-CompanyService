using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.CompanyService.Models.Dto.Enums;
using LT.DigitalOffice.CompanyService.Models.Dto.Requests.Department;
using LT.DigitalOffice.Kernel.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace CompanyService.Mappers.Db
{
    public class DbDepartmentMapper : IDbDepartmentMapper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbDepartmentMapper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbDepartment Map(CreateDepartmentRequest value, Guid companyId)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var departmentId = Guid.NewGuid();
            var authorId = _httpContextAccessor.HttpContext.GetUserId();

            var dbDepartment = new DbDepartment
            {
                Id = departmentId,
                CompanyId = companyId,
                Name = value.Name,
                Description = value.Description,
                IsActive = true,
                CreatedBy = authorId,
                CreatedAtUtc = DateTime.UtcNow,
                Users = value.Users?.Select(du =>
                    GetDbDepartmentUser(departmentId, du, authorId, DepartmentUserRole.Employee)).ToList()
                    ?? new()
            };

            if (value.DirectorUserId.HasValue)
            {
                dbDepartment.Users.Add(GetDbDepartmentUser(departmentId, value.DirectorUserId.Value, authorId, DepartmentUserRole.Director));
            }

            return dbDepartment;
        }

        private DbDepartmentUser GetDbDepartmentUser(Guid departmentId, Guid userId, Guid authorid, DepartmentUserRole role)
        {
            return new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                DepartmentId = departmentId,
                UserId = userId,
                Role = (int)role,
                IsActive = true,
                CreatedAtUtc = DateTime.UtcNow,
                CreatedBy = authorid
            };
        }
    }
}
