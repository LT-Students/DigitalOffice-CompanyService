using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Company;
using System;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
    public class DbOfficeUserMapper : IDbOfficeUserMapper
    {
        public DbOfficeUser Map(IChangeUserOfficeRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DbOfficeUser
            {
                Id = Guid.NewGuid(),
                OfficeId = request.OfficeId,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.ChangedBy,
                IsActive = true
            };
        }
    }
}
