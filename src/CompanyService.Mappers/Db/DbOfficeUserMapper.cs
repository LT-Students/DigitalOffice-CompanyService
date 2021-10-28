using System;
using LT.DigitalOffice.CompanyService.Mappers.Db.Interfaces;
using LT.DigitalOffice.CompanyService.Models.Db;
using LT.DigitalOffice.Models.Broker.Requests.Company;

namespace LT.DigitalOffice.CompanyService.Mappers.Db
{
  public class DbOfficeUserMapper : IDbOfficeUserMapper
  {
    public DbOfficeUser Map(IEditUserOfficeRequest request)
    {
      if (request == null || !request.OfficeId.HasValue)
      {
        return null;
      }

      return new DbOfficeUser
      {
        Id = Guid.NewGuid(),
        OfficeId = request.OfficeId.Value,
        UserId = request.UserId,
        CreatedAtUtc = DateTime.UtcNow,
        CreatedBy = request.ModifiedBy,
        IsActive = true
      };
    }
  }
}
