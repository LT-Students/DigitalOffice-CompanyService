using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbPositionUserMapper : IDbPositionUserMapper
    {
        public DbPositionUser Map(IChangeUserPositionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbPositionUser
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                PositionId = request.PositionId,
                IsActive = true,
                CreatedBy = request.ChangedBy,
                CreatedAtUtc = DateTime.UtcNow
            };
        }
    }
}
