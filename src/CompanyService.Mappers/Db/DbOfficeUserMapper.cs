using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbOfficeUserMapper : IDbOfficeUserMapper
    {
        public DbOfficeUser Map(Guid userId, Guid officeId, Guid modifiedBy)
        {
            return new DbOfficeUser
            {
                Id = Guid.NewGuid(),
                OfficeId = officeId,
                UserId = userId,
                CreatedAtUtc = DateTime.UtcNow,
                CreatedBy = modifiedBy,
                IsActive = true
            };
        }
    }
}
