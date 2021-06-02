using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbDepartmentUserMapper : IDbDepartmentUserMapper
    {
        public DbDepartmentUser Map(Guid departmentId, Guid userId)
        {
            return new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DepartmentId = departmentId,
                IsActive = true,
                StartTime = DateTime.UtcNow
            };
        }
    }
}
