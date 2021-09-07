using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbDepartmentUserMapper : IDbDepartmentUserMapper
    {
        public DbDepartmentUser Map(Guid userId, Guid departmentId, Guid modifiedBy)
        {
            return new DbDepartmentUser
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DepartmentId = departmentId,
                IsActive = true,
                CreatedBy = modifiedBy,
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}
