using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbPositionUserMapper : IDbPositionUserMapper
    {
        public DbPositionUser Map(Guid positionId, Guid userId)
        {
            return new DbPositionUser
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                PositionId = positionId,
                IsActive = true,
                StartTime = DateTime.UtcNow
            };
        }
    }
}
