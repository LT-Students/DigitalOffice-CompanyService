using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbPositionUserMapper : IDbPositionUserMapper
    {
        public DbPositionUser Map(Guid userId, Guid positionId, Guid modifiedBy)
        {
            return new DbPositionUser
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PositionId = positionId,
                IsActive = true,
                CreatedBy = modifiedBy,
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}
